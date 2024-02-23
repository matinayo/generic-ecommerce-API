using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.Payment;
using HalceraAPI.Models.Requests.ShoppingCart;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Customer.Controllers
{
    /// <summary>
    /// Shopping controller defining customers endpoint
    /// </summary>
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

        [HttpPost("{productId}")]
        [ProducesResponseType(typeof(APIResponse<AddToCartResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddProductToCartAsync(
            int productId,
            [FromBody] ShoppingCartRequest? shoppingCartRequest)
        {
            try
            {
                var response = await _shoppingCartOperation.AddProductToCartAsync(productId, shoppingCartRequest);

                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<ShoppingCartDetailsResponse>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetAllItemsInCartAsync(Currency currency)
        {
            try
            {
                var listOfShoppingCartItems =
                    await _shoppingCartOperation.GetAllItemsInCartAsync(currency);

                return Ok(listOfShoppingCartItems);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpGet("{shoppingCartId}")]
        [ProducesResponseType(typeof(APIResponse<ShoppingCartDetailsResponse>), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetItemInCartAsync(int shoppingCartId)
        {
            try
            {
                APIResponse<ShoppingCartDetailsResponse>? itemFromDb =
                    await _shoppingCartOperation.GetItemInCartAsync(shoppingCartId);

                return Ok(itemFromDb);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpPost("IncreaseItem/{shoppingCartId}")]
        [ProducesResponseType(typeof(APIResponse<ShoppingCartUpdateResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> IncreaseItemInCartAsync(
            int shoppingCartId,
            [FromBody] ShoppingCartRequest? shoppingCartRequest)
        {
            try
            {
                var shoppingCartUpdateResponse = await _shoppingCartOperation
                    .IncreaseItemInCartAsync(shoppingCartId, shoppingCartRequest);

                return Ok(shoppingCartUpdateResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpPost("DecreaseItem/{shoppingCartId}")]
        [ProducesResponseType(typeof(APIResponse<ShoppingCartUpdateResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DecreaseItemInCartAsync(
            int shoppingCartId,
            [FromBody] ShoppingCartRequest? shoppingCartRequest)
        {
            try
            {
                var shoppingCartUpdateResponse = await _shoppingCartOperation
                    .DecreaseItemInCartAsync(shoppingCartId, shoppingCartRequest);

                return Ok(shoppingCartUpdateResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpDelete("{shoppingCartId}")]
        [ProducesResponseType(typeof(APIResponse<ShoppingCartUpdateResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteAsync(int shoppingCartId, Currency currency)
        {
            try
            {
                var shoppingCartUpdateResponse = await _shoppingCartOperation.DeleteItemInCartAsync(shoppingCartId, currency);

                return Ok(shoppingCartUpdateResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpPost("InitializePayment")]
        [ProducesResponseType(typeof(APIResponse<InitializePaymentResponse>), 200)]
        [ProducesResponseType(400)]
        public ActionResult InitializeTransactionForCheckout(
             InitializePaymentRequest initializePaymentRequest)
        {
            try
            {
                var response = _shoppingCartOperation.InitializeTransactionForCheckout(initializePaymentRequest);

                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpPost("Checkout")]
        [ProducesResponseType(typeof(APIResponse<CheckoutResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CheckoutAsync([FromBody] CheckoutRequest checkoutRequest)
        {
            try
            {
                APIResponse<CheckoutResponse> checkoutResponse = await _shoppingCartOperation.CheckoutAsync(checkoutRequest);

                return Ok(checkoutResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }
    }
}
