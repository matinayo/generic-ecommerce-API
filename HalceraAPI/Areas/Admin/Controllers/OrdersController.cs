using HalceraAPI.Common.Utilities;
using HalceraAPI.Common.Enums;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.OrderHeader;
using HalceraAPI.Services.Dtos.Shipping;
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
        [ProducesResponseType(typeof(APIResponse<IEnumerable<OrderOverviewResponse>>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetOrdersAsync(OrderStatus? orderStatus, int? page)
        {
            APIResponse<IEnumerable<OrderOverviewResponse>> listOfOrders =
                await _orderOperation.GetOrdersAsync(orderStatus, page);

            return Ok(listOfOrders);
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(APIResponse<OrderResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> GetOrderByIdAsync(string orderId)
        {
            APIResponse<OrderResponse> orderDetails = await _orderOperation.GetOrderByIdAsync(orderId);

            return Ok(orderDetails);
        }

        [HttpPut("{orderId}/{orderStatus}")]
        [ProducesResponseType(typeof(APIResponse<UpdateOrderStatusResponse>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateOrderStatusAsync(string orderId, OrderStatus orderStatus)
        {
            APIResponse<UpdateOrderStatusResponse> updateOrderDetails =
                await _orderOperation.UpdateOrderStatusAsync(orderId, orderStatus);

            return Ok(updateOrderDetails);
        }

        [HttpPut("{orderId}/Shipping")]
        [ProducesResponseType(typeof(APIResponse<ShippingDetailsResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UpdateShippingDetailsAsync(
            string orderId, UpdateShippingDetailsRequest shippingDetailsRequest)
        {
            APIResponse<ShippingDetailsResponse> shippingDetailsResponse =
                await _orderOperation.UpdateOrderShippingDetailsAsync(orderId, shippingDetailsRequest);

            return Ok(shippingDetailsResponse);
        }

        [HttpGet("{orderId}/Shipping")]
        [ProducesResponseType(typeof(APIResponse<ShippingDetailsResponse>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> GetOrderShippingDetailsAsync(string orderId)
        {
            APIResponse<ShippingDetailsResponse> shippingDetailsResponse =
                await _orderOperation.GetOrderShippingDetailsAsync(orderId);

            return Ok(shippingDetailsResponse);
        }
    }
}
