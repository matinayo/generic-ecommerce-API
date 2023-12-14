using HalceraAPI.Common.Utilities;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.Shipping;
using HalceraAPI.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HalceraAPI.Areas.Admin.Controllers
{
    [Authorize(Roles = $"{RoleDefinition.Admin},{RoleDefinition.Employee}")]
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
        [ProducesResponseType(typeof(APIResponse<IEnumerable<OrderResponse>>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetOrdersAsync(OrderStatus? orderStatus, int? page)
        {
            try
            {
                APIResponse<IEnumerable<OrderResponse>> listOfOrders =
                    await _orderOperation.GetOrdersAsync(orderStatus, page);

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
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetOrderByIdAsync(string orderId)
        {
            try
            {
                APIResponse<OrderResponse> orderDetails = await _orderOperation.GetOrderByIdAsync(orderId);

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
        [ProducesResponseType(typeof(APIResponse<UpdateOrderStatusResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateOrderStatusAsync(string orderId, OrderStatus orderStatus)
        {
            try
            {
                APIResponse<UpdateOrderStatusResponse> updateOrderDetails =
                    await _orderOperation.UpdateOrderStatusAsync(orderId, orderStatus);

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

        [HttpPut("{orderId}/Shipping")]
        [ProducesResponseType(typeof(APIResponse<ShippingDetailsResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateShippingDetailsAsync(
            string orderId, UpdateShippingDetailsRequest shippingDetailsRequest)
        {
            try
            {
                APIResponse<ShippingDetailsResponse> shippingDetailsResponse =
                    await _orderOperation.UpdateOrderShippingDetailsAsync(orderId, shippingDetailsRequest);

                return Ok(shippingDetailsResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }

        [HttpGet("{orderId}/Shipping")]
        [ProducesResponseType(typeof(APIResponse<ShippingDetailsResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetOrderShippingDetailsAsync(string orderId)
        {
            try
            {
                APIResponse<ShippingDetailsResponse> shippingDetailsResponse =
                    await _orderOperation.GetOrderShippingDetailsAsync(orderId);

                return Ok(shippingDetailsResponse);
            }
            catch (Exception exception)
            {
                return BadRequest(
                        Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }
    }
}
