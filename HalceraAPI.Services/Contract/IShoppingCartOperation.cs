using HalceraAPI.Models;
using HalceraAPI.Models.Requests.ShoppingCart;

namespace HalceraAPI.Services.Contract
{
    /// <summary>
    /// Shopping Cart Operations
    /// </summary>
    public interface IShoppingCartOperation
    {
        /// <summary>
        /// Add product item to cart
        /// </summary>
        /// <param name="productId">id of product</param>
        /// <returns>Shopping cart details of added item in cart</returns>
        Task<ShoppingCartResponse> AddProductToCart(int productId, ShoppingCartRequest? shoppingCartRequest);
        /// <summary>
        /// Get all items in cart for user
        /// </summary>
        /// <returns>List of items in shopping cart</returns>
        Task<IEnumerable<ShoppingCartDetailsResponse>?> GetAllItemsInCart();
        /// <summary>
        /// Get specified item in cart
        /// </summary>
        /// <param name="shoppingCartId">Shopping cart Id</param>
        /// <returns>Details of Item in cart</returns>
        Task<ShoppingCartDetailsResponse?> GetItemInCart(int shoppingCartId);
        /// <summary>
        /// Increases existing item in cart
        /// </summary>
        /// <param name="shoppingCartId">Shopping cart Id</param>
        /// <param name="shoppingCartRequest">Request body</param>
        /// <returns>Updated quantity of item in shopping cart</returns>
        Task<int> IncreaseItemInCart(int shoppingCartId, ShoppingCartRequest? shoppingCartRequest);
        /// <summary>
        /// Decreases existing item in cart
        /// </summary>
        /// <param name="shoppingCartId">Shopping cart Id</param>
        /// <param name="shoppingCartRequest">Request body</param>
        /// <returns>Updated quantity of item in shopping cart</returns>
        Task<int> DecreaseItemInCart(int shoppingCartId, ShoppingCartRequest? shoppingCartRequest);
        /// <summary>
        /// Deletes item in cart
        /// </summary>
        /// <param name="shoppingCartId">Id of item in cart</param>
        /// <returns>bool to indicate if delete operation was successful</returns>
        Task<bool> DeleteItemInCart(int shoppingCartId);        
        /// <summary>
        /// User checkout, Validate payment
        /// </summary>
        /// <returns>Order Id</returns>
        Task<int> Checkout(CheckoutRequest checkoutRequest);
    }
}
