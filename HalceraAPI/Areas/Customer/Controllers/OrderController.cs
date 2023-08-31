using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader.CustomerResponse;
using HalceraAPI.Models.Requests.ShoppingCart;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICustomerOrderOperation _customerOrderOperation;
        public OrderController(ICustomerOrderOperation customerOrderOperation)
        {
            _customerOrderOperation = customerOrderOperation;
        }

        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<CustomerOrderResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<CustomerOrderResponse>>> GetAll(OrderStatus? orderStatus)        
        {
            try
            {
                IEnumerable<CustomerOrderResponse>? listOfOrders = await _customerOrderOperation.GetAllOrders(orderStatus);
                return Ok(listOfOrders);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }
    }
}
