using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader.CustomerResponse;

namespace HalceraAPI.Services.Contract
{
    public interface ICustomerOrderOperation
    {
        /// <summary>
        /// Get order details
        /// Customer is only able to view personal orders
        /// </summary>
        /// <param name="orderId">Order Id</param>
        public Task<CustomerOrderResponse> GetOrderDetails(string orderId);

        /// <summary>
        /// Get list of Orders
        /// Customer is only able to view personal orders
        /// </summary>
        /// <param name="orderStatus">Status of Orders to be retrieved</param>
        public Task<IEnumerable<CustomerOrderResponse>?> GetAllOrders(OrderStatus? orderStatus);

        /// <summary>
        /// Cancel order before Order is in Delivery process
        /// </summary>
        /// <param name="orderId">Id of Order</param>
        public void CancelOrder(string orderId);

        /// <summary>
        /// Update Order details
        /// Customer Update / Admin Update
        /// </summary>
        /// <param name="orderId">Order Id</param>
        public void UpdateOrderDetails(string orderId);
    }
}
