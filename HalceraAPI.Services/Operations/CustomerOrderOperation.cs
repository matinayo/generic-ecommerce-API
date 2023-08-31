using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader.CustomerResponse;
using HalceraAPI.Services.Contract;

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

        public void CancelOrder(string orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CustomerOrderResponse>?> GetAllOrders(OrderStatus? orderStatus)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();
                return await _unitOfWork.OrderHeader.GetAll<CustomerOrderResponse>(order => order.OrderStatus == orderStatus, 
                    orderBy: order => order.OrderBy(entity => entity.OrderDate),
                    includeProperties: $"{nameof(OrderHeader.PaymentDetails)},{nameof(OrderHeader.OrderDetails)},OrderDetails.PurchaseDetails,OrderDetails.Product,OrderDetails.Product.Prices");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CustomerOrderResponse> GetOrderDetails(string orderId)
        {
            try
            {
                ApplicationUser applicationUser = await _identityOperation.GetLoggedInUser();
                OrderHeader? orderHeaderFromDb = await _unitOfWork.OrderHeader.GetFirstOrDefault(orderHeader => orderHeader.Id.Equals(orderId, StringComparison.OrdinalIgnoreCase),
                    includeProperties: $"{nameof(OrderHeader.PaymentDetails)},{nameof(OrderHeader.OrderDetails)},OrderDetails.PurchaseDetails,OrderDetails.Product,OrderDetails.Product.Prices");
                if (orderHeaderFromDb == null)
                {
                    throw new Exception("This order cannot be found.");
                }

                return _mapper.Map<CustomerOrderResponse>(orderHeaderFromDb);
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
