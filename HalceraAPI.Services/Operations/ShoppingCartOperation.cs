using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.ShoppingCart;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class ShoppingCartOperation : IShoppingCartOperation
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartOperation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddProductToCart(int productId, ShoppingCartRequest? shoppingCartRequest)
        {
            try
            {
                ShoppingCart? cart = await _unitOfWork.ShoppingCart.GetFirstOrDefault(cart => cart.ProductId == productId, includeProperties: $"{nameof(ShoppingCart.Product)}");
                int requestedQuantity = shoppingCartRequest?.Quantity ?? 1;

                if (cart == null)
                {   // adds new item in cart
                    Product? productItem = await _unitOfWork.Product.GetFirstOrDefault(product => product.Id == productId);
                    ValidateAddingItemToCart(productItem);

                    // TODO: add ApplicationUser from Token
                    cart = new ShoppingCart() { ProductId = productId, Quantity = requestedQuantity };
                    // if product item does not exist in cart, add new item
                    await _unitOfWork.ShoppingCart.Add(cart);
                }
                else
                {
                    ValidateAndUpdateShoppingCartQuantity(cart, requestedQuantity);
                }
                await _unitOfWork.SaveAsync();
                return cart.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Decrease number of items in cart
        /// </summary>
        /// <param name="shoppingCartId">Id of item in cart</param>
        /// <returns>number of items in cart after decrease</returns>
        public async Task<int> DecreaseItemInCart(int shoppingCartId, ShoppingCartRequest? shoppingCartRequest)
        {
            try
            {
                ShoppingCart? cartItemFromDb = await _unitOfWork.ShoppingCart.GetFirstOrDefault(shoppingCart => shoppingCart.Id == shoppingCartId, includeProperties: $"{nameof(ShoppingCart.Product)}");
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
                cartItemFromDb.Quantity -= 1;
                if (cartItemFromDb.Quantity == 0)
                {
                    // remove item from cart
                    _unitOfWork.ShoppingCart.Remove(cartItemFromDb);
                }
                await _unitOfWork.SaveAsync();
                return cartItemFromDb.Quantity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Delete Item from Shopping Cart
        /// </summary>
        /// <param name="shoppingCartId">Id of shoppingCart item</param>
        /// <returns>true if item is successfully deleted</returns>
        public async Task<bool> DeleteItemInCart(int shoppingCartId)
        {
            try
            {
                ShoppingCart? cartItemFromDb = await _unitOfWork.ShoppingCart.GetFirstOrDefault(shoppingCart => shoppingCart.Id == shoppingCartId);
                if (cartItemFromDb == null)
                    throw new Exception("Item not found");
                _unitOfWork.ShoppingCart.Remove(cartItemFromDb);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get user list of shopping cart items
        /// </summary>
        /// <returns>list of shoppingCart items</returns>
        public async Task<IEnumerable<ShoppingCart>?> GetAllItemsInCart()
        {
            try
            {
                // TODO: get items for requesting user
                IEnumerable<ShoppingCart>? shoppingItemsFromDb = await _unitOfWork.ShoppingCart.GetAll();
                return shoppingItemsFromDb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get item from cart
        /// </summary>
        /// <param name="shoppingCartId">id of shoppingCart item</param>
        /// <returns>ShoppingCart item</returns>
        public async Task<ShoppingCart?> GetItemInCart(int shoppingCartId)
        {
            try
            {
                ShoppingCart? shoppingCartFromDb = await _unitOfWork.ShoppingCart.GetFirstOrDefault(shoppingCart => shoppingCart.Id == shoppingCartId);
                return shoppingCartFromDb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Increase number of items in user shoppingCart
        /// </summary>
        /// <param name="shoppingCartId">id of shopping cart item to increase</param>
        /// <returns>number of items in cart after increase</returns>
        public async Task<int> IncreaseItemInCart(int shoppingCartId, ShoppingCartRequest? shoppingCartRequest)
        {
            try
            {
                ShoppingCart? cartItemFromDb = await _unitOfWork.ShoppingCart.GetFirstOrDefault(shoppingCart => shoppingCart.Id == shoppingCartId, includeProperties: $"{nameof(ShoppingCart.Product)}");
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
                throw new Exception($"Only {cart.Product.Quantity} item(s) left");
            }
            // update quantity
            cart.Quantity = totalQuantity;
        }
    }
}