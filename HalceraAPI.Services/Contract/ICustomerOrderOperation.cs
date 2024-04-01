using HalceraAPI.Common.Enums;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.OrderHeader;
using HalceraAPI.Services.Dtos.Shipping;

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
