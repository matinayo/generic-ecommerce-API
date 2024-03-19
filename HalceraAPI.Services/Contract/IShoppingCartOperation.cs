using HalceraAPI.Common.Utilities;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.Payment;
using HalceraAPI.Models.Requests.ShoppingCart;

namespace HalceraAPI.Services.Contract
{
    public interface IShoppingCartOperation
    {
        Task<APIResponse<AddToCartResponse>> AddProductToCartAsync(int productId, ShoppingCartRequest? shoppingCartRequest);

        Task<APIResponse<ShoppingCartListResponse>> GetAllItemsInCartAsync(Currency currency = Defaults.DefaultCurrency);

        Task<APIResponse<ShoppingCartDetailsResponse>> GetItemInCartAsync(int shoppingCartId);

        Task<APIResponse<ShoppingCartUpdateResponse>> IncreaseItemInCartAsync(int shoppingCartId, ShoppingCartRequest? shoppingCartRequest);

        Task<APIResponse<ShoppingCartUpdateResponse>> DecreaseItemInCartAsync(int shoppingCartId, ShoppingCartRequest? shoppingCartRequest);

        Task<APIResponse<ShoppingCartUpdateResponse>> DeleteItemInCartAsync(int shoppingCartId, Currency currency = Defaults.DefaultCurrency);

        APIResponse<InitializePaymentResponse> InitializeTransactionForCheckout(
            InitializePaymentRequest initializePaymentRequest);

        APIResponse<VerifyPaymentResponse> VerifyTransaction(VerifyPaymentRequest verifyPaymentRequest);

        Task<APIResponse<CheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest);
    }
}
