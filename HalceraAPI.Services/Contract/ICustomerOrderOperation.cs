using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.Shipping;

namespace HalceraAPI.Services.Contract
{
    public interface ICustomerOrderOperation
    {
        Task<APIResponse<OrderResponse>> GetOrderByIdAsync(string orderId);

        Task<APIResponse<IEnumerable<OrderOverviewResponse>>> GetOrdersAsync(OrderStatus? orderStatus, int? page);

        Task<APIResponse<UpdateOrderStatusResponse>> CancelOrderAsync(string orderId);

        Task<APIResponse<ShippingDetailsResponse>> UpdateOrderShippingAddressAsync(
            string orderId, UpdateShippingAddressRequest shippingAddressRequest);
    }
}
