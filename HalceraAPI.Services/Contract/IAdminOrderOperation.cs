using HalceraAPI.Common.Enums;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.OrderHeader;
using HalceraAPI.Services.Dtos.Shipping;

namespace HalceraAPI.Services.Contract
{
    public interface IAdminOrderOperation
    {
        Task<APIResponse<OrderResponse>> GetOrderByIdAsync(string orderId);
        Task<APIResponse<IEnumerable<OrderOverviewResponse>>> GetOrdersAsync(OrderStatus? orderStatus, int? page);
        Task<APIResponse<UpdateOrderStatusResponse>> UpdateOrderStatusAsync(string orderId, OrderStatus orderStatus);
        Task<APIResponse<ShippingDetailsResponse>> UpdateOrderShippingDetailsAsync(
            string orderId, UpdateShippingDetailsRequest shippingDetailsRequest);
        Task<APIResponse<ShippingDetailsResponse>> GetOrderShippingDetailsAsync(string orderId);
    }
}
