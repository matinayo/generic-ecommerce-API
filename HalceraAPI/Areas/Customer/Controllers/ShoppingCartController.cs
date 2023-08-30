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
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartOperation _shoppingCartOperation;
        public ShoppingCartController(IShoppingCartOperation shoppingCartOperation)
        {
            _shoppingCartOperation = shoppingCartOperation;
        }

        /// <summary>
        /// Add product to shopping cart
        /// </summary>
        /// <param name="productId">Id of product</param>
        /// <returns>id of item in cart</returns>
        [HttpPost]
        [Route("AddProductToCart/{productId}")]
        [ProducesResponseType(typeof(ShoppingCartResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ShoppingCartResponse?>> AddProductToCart(int productId, [FromBody] ShoppingCartRequest? shoppingCartRequest)
        {
            try
            {
                var response = await _shoppingCartOperation.AddProductToCart(productId, shoppingCartRequest);
                return Ok(response);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        /// <summary>
        /// Get all items in cart
        /// </summary>
        /// <returns>List of shopping cart items</returns>
        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<ShoppingCartDetailsResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ShoppingCartDetailsResponse>?>> GetAllItemsInCart()
        {
            try
            {
                IEnumerable<ShoppingCartDetailsResponse>? listOfShoppingCartItems = await _shoppingCartOperation.GetAllItemsInCart();
                return Ok(listOfShoppingCartItems);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpGet]
        [Route("GetItem/{shoppingCartId}")]
        [ProducesResponseType(typeof(ShoppingCartDetailsResponse), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ShoppingCartDetailsResponse?>> GetItemInCart(int shoppingCartId)
        {
            try
            {
                ShoppingCartDetailsResponse? itemFromDb = await _shoppingCartOperation.GetItemInCart(shoppingCartId);
                return Ok(itemFromDb);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpPost]
        [Route("IncreaseItem/{shoppingCartId}")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int?>> IncreaseItemInCart(int shoppingCartId, [FromBody] ShoppingCartRequest? shoppingCartRequest)
        {
            try
            {
                int totalNumberOfItemsInCart = await _shoppingCartOperation.IncreaseItemInCart(shoppingCartId, shoppingCartRequest);
                return Ok(totalNumberOfItemsInCart);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpPost]
        [Route("DecreaseItem/{shoppingCartId}")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int?>> DecreaseItemInCart(int shoppingCartId, [FromBody] ShoppingCartRequest? shoppingCartRequest)
        {
            try
            {
                int totalNumberOfItemsInCart = await _shoppingCartOperation.DecreaseItemInCart(shoppingCartId, shoppingCartRequest);
                return Ok(totalNumberOfItemsInCart);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpDelete]
        [Route("DeleteItem/{shoppingCartId}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int?>> Delete(int shoppingCartId)
        {
            try
            {
                bool result = await _shoppingCartOperation.DeleteItemInCart(shoppingCartId);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpPost]
        [Route("Checkout")]
        [ProducesResponseType(typeof(CheckoutResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<CheckoutResponse?>> Checkout([FromBody] CheckoutRequest checkoutRequest)
        {
            try
            {
                CheckoutResponse checkoutResponse = await _shoppingCartOperation.Checkout(checkoutRequest);
                return Ok(checkoutResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }
    }
}
