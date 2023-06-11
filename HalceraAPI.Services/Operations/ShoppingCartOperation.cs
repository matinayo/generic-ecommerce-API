using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
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

        /// <summary>
        /// Decrease number of items in cart
        /// </summary>
        /// <param name="shoppingCartId">Id of item in cart</param>
        /// <returns>number of items in cart after decrease</returns>
        public async Task<int> DecreaseItemInCart(int shoppingCartId)
        {
            try
            {
                ShoppingCart? cartItemFromDb = await _unitOfWork.ShoppingCart.GetFirstOrDefault(shoppingCart => shoppingCart.Id == shoppingCartId);
                if (cartItemFromDb == null)
                    throw new Exception("Item not found");

                // decrease number of items in cart
                cartItemFromDb.Count -= 1;
                if (cartItemFromDb.Count == 0)
                {
                    // remove item from cart
                    _unitOfWork.ShoppingCart.Remove(cartItemFromDb);
                }
                await _unitOfWork.SaveAsync();
                return cartItemFromDb.Count;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
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
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
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
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
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
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Increase number of items in user shoppingCart
        /// </summary>
        /// <param name="shoppingCartId">id of shopping cart item to increase</param>
        /// <returns>number of items in cart after increase</returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> IncreaseItemInCart(int shoppingCartId)
        {
            try
            {
                ShoppingCart? cartItemFromDb = await _unitOfWork.ShoppingCart.GetFirstOrDefault(shoppingCart => shoppingCart.Id == shoppingCartId);
                if (cartItemFromDb == null)
                    throw new Exception("Item not found");
                cartItemFromDb.Count += 1;

                await _unitOfWork.SaveAsync();
                return cartItemFromDb.Count;
            }
            catch(Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
