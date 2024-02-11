using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.ShoppingCart;

namespace HalceraAPI.Services.Contract
{
    public interface IShoppingCartOperation
    {
        Task<APIResponse<AddToCartResponse>> AddProductToCartAsync(int productId, ShoppingCartRequest? shoppingCartRequest);

        Task<APIResponse<ShoppingCartListResponse>> GetAllItemsInCartAsync(Currency currency = Currency.NGN);

        Task<APIResponse<ShoppingCartDetailsResponse>> GetItemInCartAsync(int shoppingCartId);

        Task<int> IncreaseItemInCartAsync(int shoppingCartId, ShoppingCartRequest? shoppingCartRequest);

        Task<int> DecreaseItemInCartAsync(int shoppingCartId, ShoppingCartRequest? shoppingCartRequest);

        Task<bool> DeleteItemInCartAsync(int shoppingCartId);

        Task<APIResponse<CheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest);
    }
}
