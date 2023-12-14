using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.Shipping;

namespace HalceraAPI.Services.Contract
{
    public interface IAdminOrderOperation
    {
        Task<APIResponse<OrderResponse>> GetOrderByIdAsync(string orderId);
        Task<APIResponse<IEnumerable<OrderResponse>>> GetOrdersAsync(OrderStatus? orderStatus, int? page);
        Task<APIResponse<UpdateOrderStatusResponse>> UpdateOrderStatusAsync(string orderId, OrderStatus orderStatus);
        Task<APIResponse<ShippingDetailsResponse>> UpdateOrderShippingDetailsAsync(
            string orderId, UpdateShippingDetailsRequest shippingDetailsRequest);
        Task<APIResponse<ShippingDetailsResponse>> GetOrderShippingDetailsAsync(string orderId);
    }
}
