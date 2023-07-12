using HalceraAPI.Models;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Customer.Controllers
{
    /// <summary>
    /// Shopping controller defining customers endpoint
    /// </summary>
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
        /// Get list of items in cart
        /// </summary>
        /// <returns>List of shopping cart items</returns>
        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<ShoppingCart>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ShoppingCart>?>> GetAllItemsInCart()
        {
            try
            {
                IEnumerable<ShoppingCart>? listOfShoppingCartItems = await _shoppingCartOperation.GetAllItemsInCart();
                return Ok(listOfShoppingCartItems);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }


        [HttpGet]
        [Route("GetItem/{shoppingCartId}")]
        [ProducesResponseType(typeof(ShoppingCart), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ShoppingCart?>> GetItemInCart(int shoppingCartId)
        {
            try
            {
                ShoppingCart? itemFromDb = await _shoppingCartOperation.GetItemInCart(shoppingCartId);
                return Ok(itemFromDb);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }


        [HttpPost]
        [Route("IncreaseItem")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int?>> IncreaseItemInCart([FromQuery] int shoppingCartId)
        {
            try
            {
                int totalNumberOfItemsInCart = await _shoppingCartOperation.IncreaseItemInCart(shoppingCartId);
                return Ok(totalNumberOfItemsInCart);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [HttpPost]
        [Route("DecreaseItem")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int?>> DecreaseItemInCart([FromQuery] int shoppingCartId)
        {
            try
            {
                int totalNumberOfItemsInCart = await _shoppingCartOperation.DecreaseItemInCart(shoppingCartId);
                return Ok(totalNumberOfItemsInCart);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }

        [HttpDelete]
        [Route("DeleteItem")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<int?>> Delete([FromQuery] int shoppingCartId)
        {
            try
            {
                bool result = await _shoppingCartOperation.DeleteItemInCart(shoppingCartId);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.Message));
            }
        }
    }
}
