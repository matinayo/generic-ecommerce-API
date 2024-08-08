using AutoMapper;
using HalceraAPI.Common.Enums;
using HalceraAPI.Common.Utilities;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.OrderHeader;
using HalceraAPI.Services.Dtos.Shipping;
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

        public async Task<APIResponse<IEnumerable<OrderOverviewResponse>>> GetOrdersAsync(OrderStatus? orderStatus, int? page)
        {
            Expression<Func<OrderHeader, bool>>? filterExpression =
                 orderStatus != null ? order => order.OrderStatus == orderStatus : null;

            int totalItems = await _unitOfWork.OrderHeader.CountAsync(filterExpression);
            var response = await _unitOfWork.OrderHeader.GetAll<OrderOverviewResponse>(
                filter: filterExpression,
                orderBy: order => order.OrderBy(entity => entity.OrderDate),
                skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                take: Pagination.DefaultItemsPerPage);

            var meta = new Meta(totalItems, Pagination.DefaultItemsPerPage, page ?? 1);

            return new APIResponse<IEnumerable<OrderOverviewResponse>>(response, meta);
        }

        public async Task<APIResponse<OrderResponse>> GetOrderByIdAsync(string orderId)
        {
            OrderResponse orderHeaderFromDb =
                await _unitOfWork.OrderHeader.GetFirstOrDefault<OrderResponse>(
                    filter: order => order.Id.ToLower().Equals(orderId.ToLower()))
                ?? throw new Exception("Order cannot be found");

            return new APIResponse<OrderResponse>(orderHeaderFromDb);
        }

        public async Task<APIResponse<ShippingDetailsResponse>> UpdateOrderShippingDetailsAsync(
            string orderId, UpdateShippingDetailsRequest shippingDetailsRequest)
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

        public async Task<APIResponse<ShippingDetailsResponse>> GetOrderShippingDetailsAsync(string orderId)
        {
            OrderHeader orderHeader = await _unitOfWork.OrderHeader.GetFirstOrDefault(
                filter: orderHeader => orderHeader.Id.ToLower().Equals(orderId.ToLower()),
                includeProperties: "ShippingDetails.ShippingAddress")
                ?? throw new Exception("Order cannot be found");

            var response = _mapper.Map<ShippingDetailsResponse>(orderHeader.ShippingDetails ??= new());

            return new APIResponse<ShippingDetailsResponse>(response);
        }
    }
}
