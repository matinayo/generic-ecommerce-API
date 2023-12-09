using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.Shipping;
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
                OrderHeader orderHeaderFromDb = await _unitOfWork.OrderHeader.GetFirstOrDefault(
                    filter: order => order.Id.ToLower().Equals(orderId.ToLower())) ?? throw new Exception("Order cannot be found");

                orderHeaderFromDb.CancelOrder();
                await _unitOfWork.SaveAsync();

                return new UpdateOrderStatusResponse()
                {
                    OrderId = orderHeaderFromDb.Id,
                    OrderStatus = orderHeaderFromDb.OrderStatus
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<OrderResponse>?> GetOrdersAsync(OrderStatus? orderStatus)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();
                Expression<Func<OrderHeader, bool>>? filterExpression = order => order.ApplicationUserId == applicationUser.Id;
                if (orderStatus != null)
                {
                    filterExpression = order => order.ApplicationUserId == applicationUser.Id && order.OrderStatus == orderStatus;
                }

                return await _unitOfWork.OrderHeader.GetAll<OrderResponse>(
                    filter: filterExpression,
                    orderBy: order => order.OrderBy(entity => entity.OrderDate));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OrderResponse> GetOrderByIdAsync(string orderId)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();
                OrderResponse? orderHeaderFromDb =
                    await _unitOfWork.OrderHeader.GetFirstOrDefault<OrderResponse>(
                        filter: order =>
                        applicationUser.Id == order.ApplicationUserId
                        && order.Id.ToLower().Equals(orderId.ToLower()));

                return orderHeaderFromDb ?? throw new Exception("Order cannot be found");
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

        public async Task<ShippingDetailsResponse> UpdateOrderShippingAddressAsync(string orderId, UpdateShippingAddressRequest shippingAddressRequest)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();
                OrderHeader orderHeader = await _unitOfWork.OrderHeader.GetFirstOrDefault(
                    filter: orderHeader => applicationUser.Id == orderHeader.ApplicationUserId &&
                            orderHeader.Id.ToLower().Equals(orderId.ToLower()),
                    includeProperties: "ShippingDetails.ShippingAddress") ?? throw new Exception("Order cannot be found");

                orderHeader.ShippingDetails ??= new();

                _mapper.Map(shippingAddressRequest, orderHeader.ShippingDetails.ShippingAddress);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<ShippingDetailsResponse>(orderHeader.ShippingDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
