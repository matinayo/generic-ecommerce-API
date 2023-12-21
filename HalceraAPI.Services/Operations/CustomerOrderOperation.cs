using AutoMapper;
using HalceraAPI.Common.Utilities;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
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

        public async Task<APIResponse<UpdateOrderStatusResponse>> CancelOrderAsync(string orderId)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUserAsync();
                OrderHeader orderHeaderFromDb = await _unitOfWork.OrderHeader.GetFirstOrDefault(
                    filter: order => order.Id.ToLower().Equals(orderId.ToLower())) ?? throw new Exception("Order cannot be found");

                orderHeaderFromDb.CancelOrder();
                await _unitOfWork.SaveAsync();

                var result = new UpdateOrderStatusResponse()
                {
                    OrderId = orderHeaderFromDb.Id,
                    OrderStatus = orderHeaderFromDb.OrderStatus
                };

                return new APIResponse<UpdateOrderStatusResponse>(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<IEnumerable<OrderOverviewResponse>>> GetOrdersAsync(OrderStatus? orderStatus, int? page)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUserAsync();
                Expression<Func<OrderHeader, bool>>? filterExpression = order => order.ApplicationUserId == applicationUser.Id;
                if (orderStatus != null)
                {
                    filterExpression = order => order.ApplicationUserId == applicationUser.Id && order.OrderStatus == orderStatus;
                }

                int totalItems = await _unitOfWork.OrderHeader.CountAsync(filterExpression);
                var result = await _unitOfWork.OrderHeader.GetAll<OrderOverviewResponse>(
                    filter: filterExpression,
                    orderBy: order => order.OrderBy(entity => entity.OrderDate),
                    skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                    take: Pagination.DefaultItemsPerPage);

                var meta = new Meta(totalItems, Pagination.DefaultItemsPerPage, page ?? 1);

                return new APIResponse<IEnumerable<OrderOverviewResponse>>(result, meta);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<OrderResponse>> GetOrderByIdAsync(string orderId)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUserAsync();
                OrderResponse orderHeaderFromDb =
                    await _unitOfWork.OrderHeader.GetFirstOrDefault<OrderResponse>(
                        filter: order =>
                        applicationUser.Id == order.ApplicationUserId
                        && order.Id.ToLower().Equals(orderId.ToLower())) ?? throw new Exception("Order cannot be found");

                return new APIResponse<OrderResponse>(orderHeaderFromDb);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<ShippingDetailsResponse>> UpdateOrderShippingAddressAsync(
            string orderId,
            UpdateShippingAddressRequest shippingAddressRequest)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUserAsync();
                OrderHeader orderHeader = await _unitOfWork.OrderHeader.GetFirstOrDefault(
                    filter: orderHeader => applicationUser.Id == orderHeader.ApplicationUserId &&
                            orderHeader.Id.ToLower().Equals(orderId.ToLower()),
                    includeProperties: "ShippingDetails.ShippingAddress") 
                    ?? throw new Exception("Order cannot be found");

                orderHeader.ShippingDetails ??= new();
                if (orderHeader.ShippingDetails.CanUpdateShippingAddress())
                {
                    _mapper.Map(shippingAddressRequest, orderHeader.ShippingDetails.ShippingAddress);
                    await _unitOfWork.SaveAsync();
                }

                var shippingDetailsResponse = _mapper.Map<ShippingDetailsResponse>(orderHeader.ShippingDetails);

                return new APIResponse<ShippingDetailsResponse>(shippingDetailsResponse);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
