using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class AdminOrderOperation : IAdminOrderOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminOrderOperation(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<UpdateOrderStatusResponse> UpdateOrderStatusAsync(string orderId, OrderStatus orderStatus)
        {
            try
            {
                OrderHeader orderHeader = await _unitOfWork.OrderHeader
                    .GetFirstOrDefault(order => order.Id.ToLower().Equals(orderId.ToLower()))
                    ?? throw new Exception("Order cannot be found");

                if (orderStatus == OrderStatus.Cancelled)
                {
                    orderHeader.CancelOrder();
                }
                else
                {
                    orderHeader.OrderStatus = orderStatus;
                }

                await _unitOfWork.SaveAsync();

                return new UpdateOrderStatusResponse()
                {
                    OrderId = orderId,
                    OrderStatus = orderHeader.OrderStatus
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersAsync(OrderStatus? orderStatus)
        {
            try
            {
                return await _unitOfWork.OrderHeader.GetAll<OrderResponse>(
                    filter: orderStatus != null ? order => order.OrderStatus == orderStatus : null,
                    orderBy: order => order.OrderBy(entity => entity.OrderDate));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OrderResponse> GetOrderById(string orderId)
        {
            try
            {
                OrderResponse? orderHeaderFromDb =
                    await _unitOfWork.OrderHeader.GetFirstOrDefault<OrderResponse>(
                        filter: order => order.Id.ToLower().Equals(orderId.ToLower()));

                return orderHeaderFromDb ?? throw new Exception("Order cannot be found");
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void UpdateOrderDetails(string orderId)
        {
            throw new NotImplementedException();
        }
    }
}
