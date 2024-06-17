using HalceraAPI.Common.Enums;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.Payment;
using HalceraAPI.Services.Dtos.ShoppingCart;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IShoppingCartOperation _shoppingCartOperation;

        public ShoppingCartsController(IShoppingCartOperation shoppingCartOperation)
        {
            _shoppingCartOperation = shoppingCartOperation;
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponse<AddToCartResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddProductToCartAsync(
            
            [FromBody] AddToCartRequest addToCartRequest)
        {
            var response = await _shoppingCartOperation.AddProductToCartAsync(addToCartRequest);

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<ShoppingCartDetailsResponse>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetAllItemsInCartAsync(Currency currency)
        {
            var listOfShoppingCartItems =
                await _shoppingCartOperation.GetAllItemsInCartAsync(currency);

            return Ok(listOfShoppingCartItems);
        }

        [HttpGet("{shoppingCartId}")]
        [ProducesResponseType(typeof(APIResponse<ShoppingCartDetailsResponse>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetItemInCartAsync(int shoppingCartId)
        {
            APIResponse<ShoppingCartDetailsResponse>? itemFromDb =
                await _shoppingCartOperation.GetItemInCartAsync(shoppingCartId);

            return Ok(itemFromDb);
        }

        [HttpPatch("increase/{shoppingCartId}")]
        [ProducesResponseType(typeof(APIResponse<ShoppingCartUpdateResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> IncreaseItemInCartAsync(
            int shoppingCartId,
            [FromBody] ShoppingCartRequest? shoppingCartRequest)
        {
            var shoppingCartUpdateResponse = await _shoppingCartOperation
                .IncreaseItemInCartAsync(shoppingCartId, shoppingCartRequest);

            return Ok(shoppingCartUpdateResponse);
        }

        [HttpPatch("decrease/{shoppingCartId}")]
        [ProducesResponseType(typeof(APIResponse<ShoppingCartUpdateResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DecreaseItemInCartAsync(
            int shoppingCartId,
            [FromBody] ShoppingCartRequest? shoppingCartRequest)
        {
            var shoppingCartUpdateResponse = await _shoppingCartOperation
                .DecreaseItemInCartAsync(shoppingCartId, shoppingCartRequest);

            return Ok(shoppingCartUpdateResponse);
        }

        [HttpDelete("{shoppingCartId}")]
        [ProducesResponseType(typeof(APIResponse<ShoppingCartUpdateResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteAsync(int shoppingCartId, Currency currency)
        {
            var shoppingCartUpdateResponse = await _shoppingCartOperation
                .DeleteItemInCartAsync(shoppingCartId, currency);

            return Ok(shoppingCartUpdateResponse);
        }

        [HttpPost("initialize-payment")]
        [ProducesResponseType(typeof(APIResponse<InitializePaymentResponse>), 200)]
        [ProducesResponseType(400)]
        public ActionResult InitializeTransactionForCheckout(
             InitializePaymentRequest initializePaymentRequest)
        {
            var response = _shoppingCartOperation
                .InitializeTransactionForCheckout(initializePaymentRequest);

            return Ok(response);
        }

        [HttpPost("verify-payment")]
        public ActionResult<APIResponse<VerifyPaymentResponse>>
            VerifyTransaction(VerifyPaymentRequest verifyPaymentRequest)
        {
            return _shoppingCartOperation.VerifyTransaction(verifyPaymentRequest);
        }

        [HttpPost("checkout")]
        [ProducesResponseType(typeof(APIResponse<CheckoutResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CheckoutAsync([FromBody] CheckoutRequest checkoutRequest)
        {
            APIResponse<CheckoutResponse> checkoutResponse = await _shoppingCartOperation.CheckoutAsync(checkoutRequest);

            return Ok(checkoutResponse);
        }
    }
}
