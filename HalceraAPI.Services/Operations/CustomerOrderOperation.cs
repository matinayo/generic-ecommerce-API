using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.OrderHeader.CustomerResponse;
using HalceraAPI.Services.Contract;
using System.Linq.Expressions;

namespace HalceraAPI.Services.Operations
{
    public class CustomerOrderOperation : ICustomerOrderOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIdentityOperation _identityOperation;
        private readonly IMapper _mapper;

        public CustomerOrderOperation(IUnitOfWork unitOfWork, IIdentityOperation identityOperation, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _identityOperation = identityOperation;
            _mapper = mapper;
        }

        public async Task<UpdateOrderStatusResponse> CancelOrderAsync(string orderId)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();
                // Only Pending orders are eligble for cancellation
                OrderHeader? orderHeaderFromDb = await _unitOfWork.OrderHeader.GetFirstOrDefault(
                    filter: order => order.Id.ToLower().Equals(orderId.ToLower()));

                if (orderHeaderFromDb == null)
                {
                    throw new Exception("Uh-oh! It appears that this order has pulled off a magic trick and disappeared into thin air!");
                }
                if (orderHeaderFromDb.OrderStatus != OrderStatus.Pending)
                {
                    throw new Exception($"This order is like a determined superhero – once it's {orderHeaderFromDb.OrderStatus}, there's no turning back!");
                }

                orderHeaderFromDb.OrderStatus = OrderStatus.Cancelled;
                await _unitOfWork.SaveAsync();

                return new UpdateOrderStatusResponse() {
                    OrderId = orderHeaderFromDb.Id,
                    OrderStatus = orderHeaderFromDb.OrderStatus};
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CustomerOrderResponse>?> GetOrdersAsync(OrderStatus? orderStatus)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();
                Expression<Func<OrderHeader, bool>>? filterExpression = order => order.ApplicationUserId == applicationUser.Id;
                if (orderStatus != null)
                {
                    filterExpression = order => order.ApplicationUserId == applicationUser.Id && order.OrderStatus == orderStatus;
                }

                return await _unitOfWork.OrderHeader.GetAll<CustomerOrderResponse>(
                    filter: filterExpression,
                    orderBy: order => order.OrderBy(entity => entity.OrderDate),
                    includeProperties:
                        $"{nameof(OrderHeader.PaymentDetails)},{nameof(OrderHeader.OrderDetails)},OrderDetails.PurchaseDetails,OrderDetails.Product,OrderDetails.Product.Prices,OrderHeader.ShippingDetails,OrderHeader.ShippingDetails.ShippingAddress");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CustomerOrderResponse> GetOrderByIdAsync(string orderId)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();
                CustomerOrderResponse? orderHeaderFromDb = await _unitOfWork.OrderHeader.GetFirstOrDefault<CustomerOrderResponse>(
                    filter: order => applicationUser.Id == order.ApplicationUserId && order.Id.ToLower().Equals(orderId.ToLower()),
                    includeProperties:
                        $"{nameof(OrderHeader.PaymentDetails)},{nameof(OrderHeader.OrderDetails)},OrderDetails.PurchaseDetails,OrderDetails.Product,OrderDetails.Product.Prices");

                if (orderHeaderFromDb == null)
                {
                    throw new Exception("Uh-oh! It appears that this order has pulled off a magic trick and disappeared into thin air!");
                }

                return orderHeaderFromDb;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateOrderAsync(string orderId)
        {
            throw new NotImplementedException();
        }
    }
}
