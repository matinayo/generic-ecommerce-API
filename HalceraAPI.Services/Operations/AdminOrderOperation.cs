using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models.Enums;
using HalceraAPI.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Operations
{
    public class AdminOrderOperation : IAdminOrderOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminOrderOperation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CancelOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public void GetAllOrders(OrderStatus? orderStatus)
        {
            throw new NotImplementedException();
        }

        public void GetOrderDetails(string orderId)
        {
            throw new NotImplementedException();
        }

        public void ProcessOrder()
        {
            throw new NotImplementedException();
        }

        public void ShipOrder()
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderDetails(string orderId)
        {
            throw new NotImplementedException();
        }

        Task IAdminOrderOperation.GetOrderDetails(string orderId)
        {
            throw new NotImplementedException();
        }
    }
}
