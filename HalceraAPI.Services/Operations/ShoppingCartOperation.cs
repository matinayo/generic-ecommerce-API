using AutoMapper;
using HalceraAPI.Common.AppsettingsOptions;
using HalceraAPI.Common.Enums;
using HalceraAPI.Common.Utilities;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.BaseAddress;
using HalceraAPI.Services.Dtos.Payment;
using HalceraAPI.Services.Dtos.ShoppingCart;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PayStack.Net;

namespace HalceraAPI.Services.Operations
{
    public class ShoppingCartOperation : IShoppingCartOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IIdentityOperation _identityOperation;
        private readonly PaystackOptions _paystackOptions;

        public ShoppingCartOperation(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IIdentityOperation identityOperation,
            IOptions<PaystackOptions> options)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _identityOperation = identityOperation;
            _paystackOptions = options.Value;
        }

        public async Task<APIResponse<AddToCartResponse>> AddProductToCartAsync(AddToCartRequest addToCartRequest)
        {
            ApplicationUser applicationUser = await _identityOperation.GetLoggedInUserAsync();
            ShoppingCart? cart = await _unitOfWork.ShoppingCart.GetFirstOrDefault(
                cart => cart.ApplicationUserId != null
                && cart.ApplicationUserId.Equals(applicationUser.Id)
                && cart.ProductId == addToCartRequest.ProductId,
                includeProperties: $"{nameof(ShoppingCart.Product)},Product.Compositions,Composition.Sizes");

            if (cart == null)
            {
                var productItem = await _unitOfWork.Product.GetFirstOrDefault(
                    product => product.Id == addToCartRequest!.ProductId,
                    includeProperties:
                    $"{nameof(Product.Compositions)}" +
                    ",Compositions.Sizes");

                ValidateAddingItemToCart(productItem, addToCartRequest!);
                cart = _mapper.Map<ShoppingCart>(addToCartRequest);
                cart.ApplicationUserId = applicationUser.Id;

                await _unitOfWork.ShoppingCart.Add(cart);
            }
            else
            {
                ValidateAndUpdateShoppingCartQuantity(cart, addToCartRequest);
            }

            await _unitOfWork.SaveAsync();
            AddToCartResponse response = _mapper.Map<AddToCartResponse>(cart);

            return new APIResponse<AddToCartResponse>(response);
        }

        public async Task<APIResponse<ShoppingCartUpdateResponse>> DecreaseItemInCartAsync(
            int shoppingCartId,
            ShoppingCartRequest? shoppingCartRequest)
        {
            ApplicationUser applicationUser = await _identityOperation.GetLoggedInUserAsync();
            IEnumerable<ShoppingCart> shoppingItemsFromDb = await _unitOfWork.ShoppingCart.GetAll(
                shoppingCart =>
                shoppingCart.ApplicationUserId != null
                && shoppingCart.ApplicationUserId.Equals(applicationUser.Id),
                includeProperties: $"{nameof(ShoppingCart.Product)},Product.Compositions,Composition.Sizes,Composition.Prices")
                ?? throw new Exception("There is no item in your cart.");

            ShoppingCart? cartItemFromDb = shoppingItemsFromDb
                .FirstOrDefault(shoppingCart => shoppingCart.Id == shoppingCartId);

            if (cartItemFromDb == null || cartItemFromDb.Product == null)
                throw new Exception("Item not found");

            if (!ProductInCartIsValid(cartItemFromDb))
            {
                // remove invalid Item from Shopping Cart
                _unitOfWork.ShoppingCart.Remove(cartItemFromDb);
                await _unitOfWork.SaveAsync();
                throw new Exception("This item is not available");
            }

            int updatedTotalQuantity = cartItemFromDb.Quantity - (shoppingCartRequest?.Quantity ?? 1);
            List<ShoppingCart> updatedShoppingCarts = shoppingItemsFromDb.ToList();

            if (updatedTotalQuantity <= 0)
            {
                _unitOfWork.ShoppingCart.Remove(cartItemFromDb);
                updatedShoppingCarts.Remove(cartItemFromDb);

                updatedTotalQuantity = 0;
            }

            cartItemFromDb.Quantity = updatedTotalQuantity;
            await _unitOfWork.SaveAsync();

            Currency currency = shoppingCartRequest?.Currency ?? Defaults.DefaultCurrency;
            ShoppingCartUpdateResponse response = new()
            {
                Quantity = cartItemFromDb.Quantity,
                CartTotal = new()
                {
                    CurrencyToBePaidIn = currency,
                    TotalAmount = GetTotalAmountToBePaidDuringCheckout(updatedShoppingCarts, currency)
                }
            };

            return new APIResponse<ShoppingCartUpdateResponse>(response);
        }

        public async Task<APIResponse<ShoppingCartUpdateResponse>> DeleteItemInCartAsync(int shoppingCartId, Currency currency)
        {
            ApplicationUser applicationUser = await _identityOperation.GetLoggedInUserAsync();
            IEnumerable<ShoppingCart> shoppingItemsFromDb = await _unitOfWork.ShoppingCart.GetAll(
                shoppingCart =>
                shoppingCart.ApplicationUserId != null
                && shoppingCart.ApplicationUserId.Equals(applicationUser.Id),
                includeProperties: $"{nameof(ShoppingCart.Product)},Product.Compositions,Composition.Sizes")
                ?? throw new Exception("There is no item in your cart.");

            ShoppingCart cartItemFromDb = shoppingItemsFromDb.FirstOrDefault(
                shoppingCart => shoppingCart.Id == shoppingCartId)
                ?? throw new Exception("Item not found.");

            _unitOfWork.ShoppingCart.Remove(cartItemFromDb);
            await _unitOfWork.SaveAsync();

            var shoppingItemsAfterDelete = shoppingItemsFromDb.Where(cartItem => cartItem != cartItemFromDb).ToList();
            ShoppingCartUpdateResponse response = new()
            {
                Quantity = 0,
                CartTotal = new()
                {
                    CurrencyToBePaidIn = currency,
                    TotalAmount = GetTotalAmountToBePaidDuringCheckout(shoppingItemsAfterDelete, currency)
                }
            };

            return new APIResponse<ShoppingCartUpdateResponse>(response);
        }

        public async Task<APIResponse<ShoppingCartListResponse>> GetAllItemsInCartAsync(Currency currency)
        {
            ApplicationUser applicationUser = await _identityOperation.GetLoggedInUserAsync();
            IEnumerable<ShoppingCart>? shoppingItemsFromDb = await _unitOfWork.ShoppingCart.GetAll(
                shoppingCart =>
                shoppingCart.ApplicationUserId != null
                && shoppingCart.ApplicationUserId.Equals(applicationUser.Id),
                includeProperties: $"{nameof(ShoppingCart.Product)},Product.Compositions,Composition.Sizes,Composition.Prices");

            var shoppingDetailsResponse = _mapper.Map<IEnumerable<ShoppingCartDetailsResponse>>(shoppingItemsFromDb);
            CartTotalResponse cartTotalResponse = new()
            {
                TotalAmount = GetTotalAmountToBePaidDuringCheckout(shoppingItemsFromDb, currency),
                CurrencyToBePaidIn = currency
            };

            ShoppingCartListResponse shoppingCartListResponse = new()
            {
                CartTotal = cartTotalResponse,
                ItemsInCart = shoppingDetailsResponse
            };

            return new APIResponse<ShoppingCartListResponse>(shoppingCartListResponse);
        }

        public async Task<APIResponse<ShoppingCartDetailsResponse>> GetItemInCartAsync(int shoppingCartId)
        {
            ApplicationUser applicationUser = await _identityOperation.GetLoggedInUserAsync();
            var response = await _unitOfWork.ShoppingCart.GetFirstOrDefault<ShoppingCartDetailsResponse>(
                shoppingCart => shoppingCart.ApplicationUserId != null
                && shoppingCart.ApplicationUserId.Equals(applicationUser.Id)
                && shoppingCart.Id == shoppingCartId)
                ?? throw new Exception("Item not found");

            return new APIResponse<ShoppingCartDetailsResponse>(response);
        }

        public async Task<APIResponse<ShoppingCartUpdateResponse>> IncreaseItemInCartAsync(
            int shoppingCartId,
            ShoppingCartRequest? shoppingCartRequest)
        {
            ApplicationUser applicationUser = await _identityOperation.GetLoggedInUserAsync();
            IEnumerable<ShoppingCart> shoppingItemsFromDb = await _unitOfWork.ShoppingCart.GetAll(
                shoppingCart =>
                shoppingCart.ApplicationUserId != null
                && shoppingCart.ApplicationUserId.Equals(applicationUser.Id),
                includeProperties: $"{nameof(ShoppingCart.Product)},Product.Compositions,Composition.Sizes,Composition.Prices")
                ?? throw new Exception("There is no item in your cart.");

            ShoppingCart cartItemFromDb = shoppingItemsFromDb
                .FirstOrDefault(shoppingCart => shoppingCart.Id == shoppingCartId)
                ?? throw new Exception("Item not found.");

            ProductInCartIsValid(cartItemFromDb);
            cartItemFromDb.Quantity += (shoppingCartRequest?.Quantity ?? 1);
            await _unitOfWork.SaveAsync();

            Currency currency = shoppingCartRequest?.Currency ?? Defaults.DefaultCurrency;
            ShoppingCartUpdateResponse response = new()
            {
                Quantity = cartItemFromDb.Quantity,
                CartTotal = new()
                {
                    CurrencyToBePaidIn = currency,
                    TotalAmount = GetTotalAmountToBePaidDuringCheckout(shoppingItemsFromDb, currency)
                }
            };

            return new APIResponse<ShoppingCartUpdateResponse>(response);
        }

        public APIResponse<InitializePaymentResponse> InitializeTransactionForCheckout(
            InitializePaymentRequest initializePaymentRequest)
        {
            if (InvalidAmountForTransaction(initializePaymentRequest.Amount))
                throw new Exception("Amount should be greater than 0.");

            APIResponse<InitializePaymentResponse>? response = initializePaymentRequest.PaymentProvider switch
            {
                PaymentProvider.Paystack => InitializePaystackTransaction(
                        initializePaymentRequest.Email, initializePaymentRequest.Amount, initializePaymentRequest.Currency),
                _ => throw new Exception("Unsupported payment provider.")
            };

            return response ?? throw new Exception("Failed to initialize payment transaction.");
        }

        public APIResponse<VerifyPaymentResponse> VerifyTransaction(VerifyPaymentRequest verifyPaymentRequest)
        {
            APIResponse<VerifyPaymentResponse>? response = verifyPaymentRequest.PaymentProvider switch
            {
                PaymentProvider.Paystack => VerifyPaystackTransaction(
                        verifyPaymentRequest.Reference, verifyPaymentRequest.Currency),
                _ => throw new Exception("Unsupported payment provider.")
            };

            return response ?? throw new Exception("Failed to verify transaction.");
        }

        public async Task<APIResponse<CheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest)
        {
            ApplicationUser applicationUser = await _identityOperation.GetLoggedInUserAsync();

            IEnumerable<ShoppingCart> cartItemsFromDb = await _unitOfWork.ShoppingCart.GetAll(
                filter: shoppingCart => shoppingCart.ApplicationUserId != null
                && shoppingCart.ApplicationUserId == applicationUser.Id,
                includeProperties: $"{nameof(ShoppingCart.Product)},Product.Compositions,Composition.Sizes,Composition.Prices");

            if (cartItemsFromDb == null || cartItemsFromDb.IsNullOrEmpty())
                throw new Exception("No items found in cart");

            // TODO: Verify Delivery, Estimated delivery date
            _ = VerifyTransaction(new VerifyPaymentRequest()
            {
                Currency = checkoutRequest.PaymentDetailsRequest.Currency,
                Reference = checkoutRequest.PaymentDetailsRequest.Reference,
                PaymentProvider = checkoutRequest.PaymentDetailsRequest.PaymentProvider
            });

            OrderHeader orderHeader = new()
            {
                OrderStatus = OrderStatus.Pending,
                PaymentDetails = ProcessPaymentOrderDetails(checkoutRequest.PaymentDetailsRequest, cartItemsFromDb),
                OrderDetails = ProcessOrderDetails(cartItemsFromDb, checkoutRequest.PaymentDetailsRequest.Currency),
                ShippingDetails = ProcessShippingOrderDetails(checkoutRequest.ShippingAddress),
                ApplicationUserId = applicationUser.Id,
            };

            await _unitOfWork.OrderHeader.Add(orderHeader);

            _unitOfWork.ShoppingCart.RemoveRange(cartItemsFromDb);
            await _unitOfWork.SaveAsync();

            var result = _mapper.Map<CheckoutResponse>(orderHeader);

            return new APIResponse<CheckoutResponse>(result);
        }

        private static bool InvalidAmountForTransaction(decimal amount)
        {
            return amount <= 0;
        }

        private APIResponse<InitializePaymentResponse> InitializePaystackTransaction(string email, decimal amount, Currency currency)
        {
            if (currency != Currency.NGN)
                throw new Exception("The selected provider only supports Naira transactions.");

            int amountInKobo = ConvertAmountToKobo(amount);
            var paystackApi = new PayStackApi(_paystackOptions.SecretKey);
            TransactionInitializeResponse? response = paystackApi.Transactions.Initialize(email, amountInKobo);

            if (response != null && response.Status)
            {
                return _mapper.Map<APIResponse<InitializePaymentResponse>>(response);
            }
            else
            {
                throw new Exception(response?.Message ?? "Invalid request. Please try again");
            }
        }

        private static int ConvertAmountToKobo(decimal amountInNaira)
        {
            return Convert.ToInt16(amountInNaira * 100);
        }

        private APIResponse<VerifyPaymentResponse> VerifyPaystackTransaction(string reference, Currency currency)
        {
            if (currency != Currency.NGN)
                throw new Exception("The selected provider only supports Naira transactions.");

            var paystackApi = new PayStackApi(_paystackOptions.SecretKey);
            TransactionVerifyResponse? response = paystackApi.Transactions.Verify(reference);

            if (response != null && response.Status)
            {
                return _mapper.Map<APIResponse<VerifyPaymentResponse>>(response);
            }
            else
            {
                throw new Exception(response?.Message ?? "Invalid request. Please try again");
            }
        }

        /// <summary>
        /// Checks if a product is available for purchase, i.e. being added to cart
        /// </summary>
        private static void ValidateAddingItemToCart(Product? product, AddToCartRequest addToCartRequest)
        {
            if (product == null)
            {
                throw new Exception("Item not found");
            }
            if (!product.Active)
            {
                throw new Exception("This item is not available");
            }
            if (product.Compositions == null)
            {
                throw new Exception("Invalid product compositions.");
            }

            var selectedComposition = product.Compositions?.FirstOrDefault(u => u.Id == addToCartRequest.SelectedCompositionId);
            var selectedProductSize = selectedComposition?.Sizes?.FirstOrDefault(u => u.Id == addToCartRequest.SelectedProductSizeId);

            if (selectedProductSize?.Quantity <= 0)
            {
                throw new Exception("No item in stock");
            }
            if (addToCartRequest.Quantity > selectedProductSize?.Quantity)
            {
                throw new Exception($"Only {selectedProductSize?.Quantity} item(s) available in stock");
            }
        }

        private static void ValidateAndUpdateShoppingCartQuantity(ShoppingCart cart, AddToCartRequest addToCartRequest)
        {
            addToCartRequest.Quantity = cart.Quantity + (addToCartRequest.Quantity ?? 1);
            ValidateAddingItemToCart(cart.Product, addToCartRequest);
            cart.Quantity = addToCartRequest.Quantity ?? 1;
        }

        /// <summary>
        /// Get Info of Payment Details for Order
        /// </summary>
        /// <param name="paymentDetailsRequest">Payment Details request</param>
        /// <param name="cartItemsFromDb">Application User Shopping Cart Items including Product.Price</param>
        /// <returns>Order Payment Details</returns>
        private PaymentDetails ProcessPaymentOrderDetails(PaymentDetailsRequest paymentDetailsRequest, IEnumerable<ShoppingCart> cartItemsFromDb)
        {
            PaymentDetails paymentDetails = _mapper.Map<PaymentDetails>(paymentDetailsRequest);
            paymentDetails.TotalAmountToBePaid = GetTotalAmountToBePaidDuringCheckout(cartItemsFromDb, paymentDetailsRequest.Currency);
            // Payment status
            paymentDetails.PaymentStatus =
                paymentDetails.TotalAmountToBePaid > paymentDetails.AmountPaid ? PaymentStatus.PartialPayment : PaymentStatus.PaymentSucceeded;
            return paymentDetails;
        }

        private static decimal GetTotalAmountToBePaidDuringCheckout(IEnumerable<ShoppingCart> cartItemsFromDb, Currency currencyToBePaidIn)
        {
            decimal totalAmount = 0.0M;
            List<OrderDetails> orderDetails = new();

            foreach (var cartItem in cartItemsFromDb.Where(u => u.Product != null && u.Product.Active))
            {
                if (ProductInCartIsValid(cartItem))
                {
                    Price? productSelectedPrice = cartItem?.Composition?.Prices?.FirstOrDefault(price => price.Currency != null && price.Currency == currencyToBePaidIn);
                    if (productSelectedPrice != null)
                    {
                        // Total amount
                        decimal productAmount = productSelectedPrice.DiscountAmount ?? productSelectedPrice.Amount ?? 0M;
                        totalAmount += (productAmount * (cartItem?.Quantity ?? 1));
                    }
                }
            }

            return totalAmount;
        }

        private static bool ProductInCartIsValid(ShoppingCart cartItem)
        {
            return (
                cartItem != null
                && cartItem.Product != null
                && cartItem.Product.Active
                && cartItem.ProductSize != null
                && cartItem.ProductSize.Quantity > 0
                && cartItem.Quantity > 0);
        }

        private ShippingDetails ProcessShippingOrderDetails(AddressRequest shippingAddressRequest)
        {
            BaseAddress baseAddress = _mapper.Map<BaseAddress>(shippingAddressRequest);
            ShippingDetails shippingDetails = new()
            {
                ShippingAddress = baseAddress
            };

            return shippingDetails;
        }

        private static ICollection<OrderDetails> ProcessOrderDetails(IEnumerable<ShoppingCart> cartItemsFromDb, Currency currencyToBePaidIn)
        {
            List<OrderDetails> orderDetails = new();
            foreach (var cartItem in cartItemsFromDb)
            {
                if (cartItem != null)
                {
                    continue;
                }

                Price? selectedProductPrice = cartItem!.Composition?.Prices?.FirstOrDefault(u => u.Currency == currencyToBePaidIn);
                OrderDetails orderDetail = new()
                {
                    ProductReferenceId = cartItem!.ProductId,
                    Product = new OrderProduct()
                    {
                        Title = cartItem?.Product?.Title
                    },
                    ProductSize = new OrderProductSize()
                    {
                        Quantity = cartItem!.Quantity,
                        Size = cartItem!.ProductSize?.Size
                    },
                    Composition = new OrderComposition()
                    {
                        ColorName = cartItem!.Composition?.ColorName ?? string.Empty,
                        ColorCode = cartItem!.Composition?.ColorCode ?? string.Empty
                    },
                    PurchaseDetails = new PurchaseDetails()
                    {
                        ApplicationUserId = cartItem!.ApplicationUserId,
                        Currency = currencyToBePaidIn,
                        DiscountAmount = selectedProductPrice?.DiscountAmount,
                        ProductAmountAtPurchase = selectedProductPrice?.Amount,
                        PurchaseDate = DateTime.UtcNow
                    }
                };

                orderDetails.Add(orderDetail);
                if (cartItem.ProductSize != null)
                {
                    cartItem.ProductSize.Quantity -= cartItem.Quantity;
                }
            }

            return orderDetails;
        }

    }
}