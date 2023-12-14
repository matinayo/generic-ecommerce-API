using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.ShoppingCart;

namespace HalceraAPI.Services.Contract
{
    public interface IShoppingCartOperation
    {
        Task<APIResponse<ShoppingCartResponse>> AddProductToCartAsync(int productId, ShoppingCartRequest? shoppingCartRequest);

        Task<APIResponse<IEnumerable<ShoppingCartDetailsResponse>>> GetAllItemsInCartAsync();

        Task<APIResponse<ShoppingCartDetailsResponse>> GetItemInCartAsync(int shoppingCartId);

        Task<int> IncreaseItemInCartAsync(int shoppingCartId, ShoppingCartRequest? shoppingCartRequest);

        Task<int> DecreaseItemInCartAsync(int shoppingCartId, ShoppingCartRequest? shoppingCartRequest);

        Task<bool> DeleteItemInCartAsync(int shoppingCartId);

        Task<APIResponse<CheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest);
    }
}
