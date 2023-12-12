using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.Shipping;

namespace HalceraAPI.Services.Contract
{
    public interface ICustomerOrderOperation
    {
        /// <summary>
        /// Get order details
        /// Customer is only able to view personal orders
        /// </summary>
        /// <param name="orderId">Order Id</param>
        Task<OrderResponse> GetOrderByIdAsync(string orderId);

        /// <summary>
        /// Get list of Orders
        /// Customer is only able to view personal orders
        /// </summary>
        /// <param name="orderStatus">Status of Orders to be retrieved</param>
        Task<IEnumerable<OrderResponse>?> GetOrdersAsync(OrderStatus? orderStatus);

        /// <summary>
        /// Cancel order before Order is in Delivery process
        /// </summary>
        /// <param name="orderId">Id of Order</param>
        Task<UpdateOrderStatusResponse> CancelOrderAsync(string orderId);

        Task<ShippingDetailsResponse> UpdateOrderShippingAddressAsync(string orderId, UpdateShippingAddressRequest shippingAddressRequest);
    }
}
