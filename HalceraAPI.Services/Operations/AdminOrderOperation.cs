using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.Shipping;
using HalceraAPI.Services.Contract;

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
                else if(orderStatus == OrderStatus.Shipped)
                {
                    orderHeader.ShipOrder();
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

        public async Task<OrderResponse> GetOrderByIdAsync(string orderId)
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

        public async Task<ShippingDetailsResponse> UpdateOrderShippingDetailsAsync(string orderId, UpdateShippingDetailsRequest shippingDetailsRequest)
        {
            try
            {
                OrderHeader orderHeader = await _unitOfWork.OrderHeader.GetFirstOrDefault(
                    filter: orderHeader => orderHeader.Id.ToLower().Equals(orderId.ToLower()),
                    includeProperties: "ShippingDetails.ShippingAddress") ?? throw new Exception("Order cannot be found");

                orderHeader.ShippingDetails ??= new();
                
                    _mapper.Map(shippingDetailsRequest, orderHeader.ShippingDetails);
                    await _unitOfWork.SaveAsync();
                
                return _mapper.Map<ShippingDetailsResponse>(orderHeader.ShippingDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ShippingDetailsResponse> GetOrderShippingDetailsAsync(string orderId)
        {
            try
            {
                OrderHeader orderHeader = await _unitOfWork.OrderHeader.GetFirstOrDefault(
                    filter: orderHeader => orderHeader.Id.ToLower().Equals(orderId.ToLower()),
                    includeProperties: "ShippingDetails.ShippingAddress") ?? throw new Exception("Order cannot be found");

                return _mapper.Map<ShippingDetailsResponse>(orderHeader.ShippingDetails ??= new());
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
