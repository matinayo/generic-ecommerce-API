using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader;

namespace HalceraAPI.Services.Contract
{
    public interface IAdminOrderOperation
    {
        Task<OrderResponse> GetOrderById(string orderId);
        Task<IEnumerable<OrderResponse>> GetOrdersAsync(OrderStatus? orderStatus);
        Task<UpdateOrderStatusResponse> UpdateOrderStatusAsync(string orderId, OrderStatus orderStatus);
    }
}
