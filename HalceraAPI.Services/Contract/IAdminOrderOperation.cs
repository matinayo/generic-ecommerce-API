using HalceraAPI.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Contract
{
    public interface IAdminOrderOperation
    {
        /// <summary>
        /// Get order details
        /// Customer is only able to view personal orders
        /// </summary>
        /// <param name="orderId">Order Id</param>
        public Task GetOrderDetails(string orderId);

        /// <summary>
        /// Get list of Orders
        /// Customer is only able to view personal orders
        /// </summary>
        /// <param name="orderStatus">Status of Orders to be retrieved</param>
        public void GetAllOrders(OrderStatus? orderStatus);

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

        /// <summary>
        /// Start processing order
        /// </summary>
        public void ProcessOrder();

        /// <summary>
        /// Ship Order
        /// </summary>
        public void ShipOrder();
    }
}
