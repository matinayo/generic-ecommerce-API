using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.OrderHeader.CustomerResponse;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ICustomerOrderOperation _customerOrderOperation;
        public OrdersController(ICustomerOrderOperation customerOrderOperation)
        {
            _customerOrderOperation = customerOrderOperation;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerOrderResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<CustomerOrderResponse>>> GetAll(OrderStatus? orderStatus)
        {
            try
            {
                IEnumerable<CustomerOrderResponse>? listOfOrders = await _customerOrderOperation.GetOrdersAsync(orderStatus);

                return Ok(listOfOrders);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(IEnumerable<CustomerOrderResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<CustomerOrderResponse>>> GetOrderById(string orderId)
        {
            try
            {
                CustomerOrderResponse orderDetails = await _customerOrderOperation.GetOrderByIdAsync(orderId);

                return Ok(orderDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpGet("CancelOrder/{orderId}")]
        [ProducesResponseType(typeof(UpdateOrderStatusResponse), 200)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<UpdateOrderStatusResponse>> CancelOrder(string orderId)
        {
            try
            {
                UpdateOrderStatusResponse orderStatusUpdateResponse = await _customerOrderOperation.CancelOrderAsync(orderId);

                return Ok(orderStatusUpdateResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }
    }
}
