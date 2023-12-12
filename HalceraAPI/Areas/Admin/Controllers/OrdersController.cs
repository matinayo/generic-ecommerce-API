using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.Shipping;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IAdminOrderOperation _orderOperation;

        public OrdersController(IAdminOrderOperation orderOperation)
        {
            _orderOperation = orderOperation;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersAsync(OrderStatus? orderStatus)
        {
            try
            {
                IEnumerable<OrderResponse>? listOfOrders = await _orderOperation.GetOrdersAsync(orderStatus);

                return Ok(listOfOrders);
            }
            catch (Exception exception)
            {
                return BadRequest(
                    Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(OrderResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrderResponse>> GetOrderByIdAsync(string orderId)
        {
            try
            {
                OrderResponse orderDetails = await _orderOperation.GetOrderByIdAsync(orderId);

                return Ok(orderDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(
                    Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [HttpPut("{orderId}/{orderStatus}")]
        [ProducesResponseType(typeof(UpdateOrderStatusResponse), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UpdateOrderStatusResponse>> UpdateOrderStatusAsync(string orderId, OrderStatus orderStatus)
        {
            try
            {
                UpdateOrderStatusResponse updateOrderDetails = await _orderOperation.UpdateOrderStatusAsync(orderId, orderStatus);

                return Ok(updateOrderDetails);
            }
            catch (Exception exception)
            {
                return BadRequest(
                    Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [HttpPut("{orderId}/shipping")]
        [ProducesResponseType(typeof(ShippingDetailsResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ShippingDetailsResponse>> UpdateShippingDetailsAsync(string orderId, UpdateShippingDetailsRequest shippingDetailsRequest)
        {
            try
            {
                ShippingDetailsResponse shippingDetailsResponse = await _orderOperation.UpdateOrderShippingDetailsAsync(orderId, shippingDetailsRequest);

                return Ok(shippingDetailsResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }

        [HttpGet("{orderId}/shipping")]
        [ProducesResponseType(typeof(ShippingDetailsResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ShippingDetailsResponse>> GetOrderShippingDetailsAsync(string orderId)
        {
            try
            {
                ShippingDetailsResponse shippingDetailsResponse = await _orderOperation.GetOrderShippingDetailsAsync(orderId);

                return Ok(shippingDetailsResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(Problem(statusCode: StatusCodes.Status400BadRequest, detail: exception.InnerException?.Message ?? exception.Message));
            }
        }
    }
}
