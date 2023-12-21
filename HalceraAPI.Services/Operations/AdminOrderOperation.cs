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
    public class AdminOrderOperation : IAdminOrderOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdminOrderOperation(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<APIResponse<UpdateOrderStatusResponse>> UpdateOrderStatusAsync(string orderId, OrderStatus orderStatus)
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
                else if (orderStatus == OrderStatus.Shipped)
                {
                    orderHeader.ShipOrder();
                }
                else
                {
                    orderHeader.OrderStatus = orderStatus;
                }

                await _unitOfWork.SaveAsync();
                var response = new UpdateOrderStatusResponse()
                {
                    OrderId = orderId,
                    OrderStatus = orderHeader.OrderStatus
                };

                return new APIResponse<UpdateOrderStatusResponse>(response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<IEnumerable<OrderResponse>>> GetOrdersAsync(OrderStatus? orderStatus, int? page)
        {
            try
            {
                Expression<Func<OrderHeader, bool>>? filterExpression =
                     orderStatus != null ? order => order.OrderStatus == orderStatus : null;

                int totalItems = await _unitOfWork.OrderHeader.CountAsync(filterExpression);
                var response = await _unitOfWork.OrderHeader.GetAll<OrderResponse>(
                    filter: filterExpression,
                    orderBy: order => order.OrderBy(entity => entity.OrderDate),
                    skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                    take: Pagination.DefaultItemsPerPage);

                var meta = new Meta(totalItems, Pagination.DefaultItemsPerPage, page ?? 1);

                return new APIResponse<IEnumerable<OrderResponse>>(response, meta);
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
                OrderResponse orderHeaderFromDb =
                    await _unitOfWork.OrderHeader.GetFirstOrDefault<OrderResponse>(
                        filter: order => order.Id.ToLower().Equals(orderId.ToLower()))
                    ?? throw new Exception("Order cannot be found");

                return new APIResponse<OrderResponse>(orderHeaderFromDb);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<ShippingDetailsResponse>> UpdateOrderShippingDetailsAsync(
            string orderId, UpdateShippingDetailsRequest shippingDetailsRequest)
        {
            try
            {
                OrderHeader orderHeader = await _unitOfWork.OrderHeader.GetFirstOrDefault(
                    filter: orderHeader => orderHeader.Id.ToLower().Equals(orderId.ToLower()),
                    includeProperties: "ShippingDetails.ShippingAddress") ?? throw new Exception("Order cannot be found");

                orderHeader.ShippingDetails ??= new();

                _mapper.Map(shippingDetailsRequest, orderHeader.ShippingDetails);
                await _unitOfWork.SaveAsync();

                var response = _mapper.Map<ShippingDetailsResponse>(orderHeader.ShippingDetails);

                return new APIResponse<ShippingDetailsResponse>(response);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<ShippingDetailsResponse>> GetOrderShippingDetailsAsync(string orderId)
        {
            try
            {
                OrderHeader orderHeader = await _unitOfWork.OrderHeader.GetFirstOrDefault(
                    filter: orderHeader => orderHeader.Id.ToLower().Equals(orderId.ToLower()),
                    includeProperties: "ShippingDetails.ShippingAddress") 
                    ?? throw new Exception("Order cannot be found");

                var response = _mapper.Map<ShippingDetailsResponse>(orderHeader.ShippingDetails ??= new());

                return new APIResponse<ShippingDetailsResponse>(response);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
