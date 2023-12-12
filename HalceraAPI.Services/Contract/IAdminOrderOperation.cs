using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.Shipping;

namespace HalceraAPI.Services.Contract
{
    public interface IAdminOrderOperation
    {
        Task<OrderResponse> GetOrderByIdAsync(string orderId);
        Task<IEnumerable<OrderResponse>> GetOrdersAsync(OrderStatus? orderStatus);
        Task<UpdateOrderStatusResponse> UpdateOrderStatusAsync(string orderId, OrderStatus orderStatus);
        Task<ShippingDetailsResponse> UpdateOrderShippingDetailsAsync(string orderId, UpdateShippingDetailsRequest shippingDetailsRequest);
        Task<ShippingDetailsResponse> GetOrderShippingDetailsAsync(string orderId);
    }
}
