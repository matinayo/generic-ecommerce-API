using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.BaseAddress;
using HalceraAPI.Models.Requests.ShoppingCart;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class ShoppingCartOperation : IShoppingCartOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IIdentityOperation _identityOperation;

        public ShoppingCartOperation(IUnitOfWork unitOfWork, IMapper mapper, IIdentityOperation identityOperation)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _identityOperation = identityOperation;
        }

        public async Task<ShoppingCartResponse> AddProductToCart(int productId, ShoppingCartRequest? shoppingCartRequest)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();

                ShoppingCart? cart = await _unitOfWork.ShoppingCart.GetFirstOrDefault(cart => cart.ApplicationUserId != null && cart.ApplicationUserId.Equals(applicationUser.Id) && cart.ProductId == productId, includeProperties: $"{nameof(ShoppingCart.Product)}");
                int requestedQuantity = shoppingCartRequest?.Quantity ?? 1;

                if (cart == null)
                {   // adds new item in cart
                    Product? productItem = await _unitOfWork.Product.GetFirstOrDefault(product => product.Id == productId);
                    ValidateAddingItemToCart(productItem);
                    if(requestedQuantity > productItem?.Quantity)
                    {
                        throw new Exception($"Only {productItem?.Quantity} item(s) available in stock");
                    }

                    cart = new ShoppingCart() { ProductId = productId, Quantity = requestedQuantity, ApplicationUserId = applicationUser.Id };
                    // if product item does not exist in cart, add new item
                    await _unitOfWork.ShoppingCart.Add(cart);
                }
                else
                {
                    ValidateAndUpdateShoppingCartQuantity(cart, requestedQuantity);
                }
                await _unitOfWork.SaveAsync();

                ShoppingCartResponse response = _mapper.Map<ShoppingCartResponse>(cart);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DecreaseItemInCart(int shoppingCartId, ShoppingCartRequest? shoppingCartRequest)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();

                ShoppingCart? cartItemFromDb = await _unitOfWork.ShoppingCart.GetFirstOrDefault(shoppingCart => shoppingCart.ApplicationUserId != null && shoppingCart.ApplicationUserId.Equals(applicationUser.Id) && shoppingCart.Id == shoppingCartId, includeProperties: $"{nameof(ShoppingCart.Product)}");
                if (cartItemFromDb == null || cartItemFromDb.Product == null)
                    throw new Exception("Item not found");

                if (!cartItemFromDb.Product.Active || cartItemFromDb.Quantity <= 0)
                {
                    // remove from invalid Item from Shopping Cart
                    _unitOfWork.ShoppingCart.Remove(cartItemFromDb);
                    await _unitOfWork.SaveAsync();
                    throw new Exception("This item is not available");
                }

                // decrease number of items in cart
                int totalQuantityToRemove = cartItemFromDb.Quantity - (shoppingCartRequest?.Quantity ?? 1);
                if(totalQuantityToRemove <= 0)
                {
                    // remove item from cart
                    _unitOfWork.ShoppingCart.Remove(cartItemFromDb);
                    totalQuantityToRemove = 0;
                }
                cartItemFromDb.Quantity = totalQuantityToRemove;
                
                await _unitOfWork.SaveAsync();
                return cartItemFromDb.Quantity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteItemInCart(int shoppingCartId)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();
                ShoppingCart? cartItemFromDb = await _unitOfWork.ShoppingCart.GetFirstOrDefault(shoppingCart => shoppingCart.ApplicationUserId != null && shoppingCart.ApplicationUserId.Equals(applicationUser.Id) && shoppingCart.Id == shoppingCartId);
                if (cartItemFromDb == null)
                    throw new Exception("Item not found");
                _unitOfWork.ShoppingCart.Remove(cartItemFromDb);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ShoppingCartDetailsResponse>?> GetAllItemsInCart()
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();

                IEnumerable<ShoppingCart>? shoppingItemsFromDb = await _unitOfWork.ShoppingCart.GetAll(
                    shoppingCart => shoppingCart.ApplicationUserId != null && shoppingCart.ApplicationUserId.Equals(applicationUser.Id),
                    includeProperties: $"{nameof(ShoppingCart.Product)},Product.Categories,Product.MediaCollection,Product.Prices");
                var response = _mapper.Map<IEnumerable<ShoppingCartDetailsResponse>>(shoppingItemsFromDb);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ShoppingCartDetailsResponse?> GetItemInCart(int shoppingCartId)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();
                ShoppingCart? shoppingCartFromDb = await _unitOfWork.ShoppingCart.GetFirstOrDefault(shoppingCart => shoppingCart.ApplicationUserId != null && shoppingCart.ApplicationUserId.Equals(applicationUser.Id) && shoppingCart.Id == shoppingCartId,
                    includeProperties: $"{nameof(ShoppingCart.Product)},Product.Categories,Product.MediaCollection,Product.Prices");
                if(shoppingCartFromDb == null)
                {
                    throw new Exception("Item not found");
                }
                ShoppingCartDetailsResponse response = _mapper.Map<ShoppingCartDetailsResponse>(shoppingCartFromDb);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> IncreaseItemInCart(int shoppingCartId, ShoppingCartRequest? shoppingCartRequest)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();
                ShoppingCart? cartItemFromDb = await _unitOfWork.ShoppingCart.GetFirstOrDefault(shoppingCart => shoppingCart.ApplicationUserId != null && shoppingCart.ApplicationUserId.Equals(applicationUser.Id) && shoppingCart.Id == shoppingCartId, includeProperties: $"{nameof(ShoppingCart.Product)}");
                if (cartItemFromDb == null)
                    throw new Exception("Item not found");

                int requestedQuantity = shoppingCartRequest?.Quantity ?? 1;
                ValidateAndUpdateShoppingCartQuantity(cartItemFromDb, requestedQuantity);

                await _unitOfWork.SaveAsync();
                return cartItemFromDb.Quantity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CheckoutResponse> Checkout(CheckoutRequest checkoutRequest)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();
                
                IEnumerable<ShoppingCart> cartItemsFromDb = await _unitOfWork.ShoppingCart.GetAll(
                    filter: shoppingCart => shoppingCart.ApplicationUserId != null && shoppingCart.ApplicationUserId == applicationUser.Id, 
                    includeProperties: $"{nameof(ShoppingCart.Product)},Product.Prices");

                if (cartItemsFromDb == null) throw new Exception("No items found in cart");

                // TODO: Verify Payment, Verify Delivery, Estimated delivery date
                OrderHeader orderHeader = new()
                {
                    OrderStatus = OrderStatus.Pending,
                    PaymentDetails = ProcessPaymentOrderDetails(checkoutRequest.PaymentDetailsRequest, cartItemsFromDb),
                    OrderDetails = ProcessOrderDetails(cartItemsFromDb, checkoutRequest.PaymentDetailsRequest.Currency),
                    ShippingDetails = ProcessShippingOrderDetails(checkoutRequest.ShippingAddress),
                    ApplicationUserId = applicationUser.Id,
                };
                
                await _unitOfWork.OrderHeader.Add(orderHeader);
                // Reset User Shopping Cart
                _unitOfWork.ShoppingCart.RemoveRange(cartItemsFromDb);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<CheckoutResponse>(orderHeader);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Checks if a product is available for purchase, i.e. being added to cart
        /// </summary>
        private static void ValidateAddingItemToCart(Product? product)
        {
            if (product == null)
            {
                throw new Exception("Item not found");
            }
            if (!product.Active)
            {
                throw new Exception("This item is not available");
            }
            if (product.Quantity <= 0)
            {
                throw new Exception("No item in stock");
            }
        }

        /// <summary>
        /// Validate and updates quantity of a shopping cart item with product data
        /// </summary>
        /// <param name="cart">Existing item in cart from Db</param>
        /// <param name="requestedQuantity">quantity to be added</param>
        private static void ValidateAndUpdateShoppingCartQuantity(ShoppingCart cart, int requestedQuantity)
        {
            ValidateAddingItemToCart(cart.Product);
            cart.Product ??= new();

            // update existing item count in cart
            int totalQuantity = cart.Quantity + requestedQuantity;
            if (totalQuantity > cart.Product.Quantity)
            {
                throw new Exception($"Only {cart.Product.Quantity} item(s) available in stock");
            }
            // update quantity
            cart.Quantity = totalQuantity;
        }

        /// <summary>
        /// Get Info of Payment Details for Order
        /// </summary>
        /// <param name="paymentDetailsRequest">Payment Details request</param>
        /// <param name="cartItemsFromDb">Application User Shopping Cart Items including Product.Price</param>
        /// <returns>Order Payment Details</returns>
        private PaymentDetails ProcessPaymentOrderDetails(PaymentDetailsRequest paymentDetailsRequest, IEnumerable<ShoppingCart> cartItemsFromDb)
        {
            // Payment order details
            PaymentDetails paymentDetails = _mapper.Map<PaymentDetails>(paymentDetailsRequest);
            // Total Amount to be paid and Order Header
            paymentDetails.TotalAmount = GetTotalAmountToBePaidDuringCheckout(cartItemsFromDb, paymentDetailsRequest.Currency);
            // Payment status
            paymentDetails.PaymentStatus =
                paymentDetails.TotalAmount > paymentDetails.AmountPaid ? PaymentStatus.PartialPayment : PaymentStatus.PaymentSucceeded;
            return paymentDetails;
        }

        /// <summary>
        /// Calculate total amount of Items added in Shopping Cart
        /// </summary>
        /// <param name="cartItemsFromDb">ShoppingCart Items of User Id Including Product.Price</param>
        /// <param name="currencyToBePaidIn">Currency of Product Price to be calculated</param>
        /// <returns>Product total amount</returns>
        private static decimal GetTotalAmountToBePaidDuringCheckout(IEnumerable<ShoppingCart> cartItemsFromDb, Currency currencyToBePaidIn)
        {
            decimal totalAmount = 0.0M;
            List<OrderDetails> orderDetails = new();

            foreach (var cartItem in cartItemsFromDb)
            {
                if (cartItem.Product != null && cartItem.Product.Prices != null)
                {
                    Price? productSelectedPrice = cartItem.Product.Prices.FirstOrDefault(price => price.Currency != null && price.Currency == currencyToBePaidIn);
                    if (productSelectedPrice != null)
                    {
                        // Total amount
                        decimal productAmount = productSelectedPrice.DiscountAmount ?? productSelectedPrice.Amount ?? 0M;
                        totalAmount += (productAmount * cartItem.Quantity);
                    }
                }
            }
            return totalAmount;
        }

        /// <summary>
        /// Process Order Shipping Address and Data
        /// </summary>
        /// <param name="shippingAddressRequest">Shipping Address request</param>
        /// <returns>Shipping Details</returns>
        private ShippingDetails ProcessShippingOrderDetails(AddressRequest shippingAddressRequest)
        {
            BaseAddress baseAddress = _mapper.Map<BaseAddress>(shippingAddressRequest);
            ShippingDetails shippingDetails = new()
            {
                ShippingAddress = baseAddress
            };
            return shippingDetails;
        }

        /// <summary>
        /// Process Order details
        /// </summary>
        /// <param name="cartItemsFromDb">User Cart items from including Product Price</param>
        /// <param name="currencyToBePaidIn">User checkout currency</param>
        /// <returns>List of Order Details</returns>
        private static ICollection<OrderDetails> ProcessOrderDetails(IEnumerable<ShoppingCart> cartItemsFromDb, Currency currencyToBePaidIn)
        {
            List<OrderDetails> orderDetails = new();
            foreach (var cartItem in cartItemsFromDb)
            {
                if (cartItem.Product != null && cartItem.Product.Prices != null)
                {
                    Price? productSelectedPrice = cartItem.Product.Prices.FirstOrDefault(price => price.Currency != null && price.Currency == currencyToBePaidIn);
                    if (productSelectedPrice != null)
                    {
                        // Order details
                        OrderDetails orderDetail = new()
                        {
                            ProductId = cartItem.ProductId,
                            // Purchase details
                            PurchaseDetails = new()
                            {
                                ApplicationUserId = cartItem.ApplicationUserId,
                                Currency = productSelectedPrice?.Currency,
                                DiscountAmount = productSelectedPrice?.DiscountAmount,
                                ProductAmountAtPurchase = productSelectedPrice?.Amount,
                                Quantity = cartItem.Quantity,
                                PurchaseDate = DateTime.UtcNow
                            }
                        };
                        orderDetails.Add(orderDetail);
                        // Update product quantity in stock
                        cartItem.Product.Quantity -= cartItem.Quantity;
                    }
                }
            }
            return orderDetails;
        }
    }
}