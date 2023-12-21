using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.Shipping;
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
        [ProducesResponseType(typeof(APIResponse<IEnumerable<OrderOverviewResponse>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetAll(OrderStatus? orderStatus, int? page)
        {
            try
            {
                APIResponse<IEnumerable<OrderOverviewResponse>> listOfOrders = 
                    await _customerOrderOperation.GetOrdersAsync(orderStatus, page);

                return Ok(listOfOrders);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<OrderResponse>>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetOrderById(string orderId)
        {
            try
            {
                APIResponse<OrderResponse> orderDetails = await _customerOrderOperation.GetOrderByIdAsync(orderId);

                return Ok(orderDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest, 
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpGet("Cancel/{orderId}")]
        [ProducesResponseType(typeof(APIResponse<UpdateOrderStatusResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> CancelOrder(string orderId)
        {
            try
            {
                APIResponse<UpdateOrderStatusResponse> orderStatusUpdateResponse = 
                    await _customerOrderOperation.CancelOrderAsync(orderId);

                return Ok(orderStatusUpdateResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest, 
                            detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpPut("{orderId}/Shipping")]
        [ProducesResponseType(typeof(APIResponse<ShippingDetailsResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateShippingAddress(string orderId, UpdateShippingAddressRequest shippingAddressRequest)
        {
            try
            {
                APIResponse<ShippingDetailsResponse> shippingDetailsResponse = 
                    await _customerOrderOperation.UpdateOrderShippingAddressAsync(orderId, shippingAddressRequest);

                return Ok(shippingDetailsResponse);
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
