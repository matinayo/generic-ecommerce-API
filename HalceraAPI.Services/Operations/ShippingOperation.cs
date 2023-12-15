using AutoMapper;
using HalceraAPI.Common.Utilities;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.Shipping;
using HalceraAPI.Services.Contract;
using System.Linq.Expressions;

namespace HalceraAPI.Services.Operations
{
    public class ShippingOperation : IShippingOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShippingOperation(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<APIResponse<IEnumerable<ShippingDetailsResponse>>> GetAllShippingRequestsAsync(
            ShippingStatus? shippingStatus, int? page)
        {
            try
            {
                Expression<Func<ShippingDetails, bool>>? filterExpression =
                    shippingStatus != null ? shipping => shipping.ShippingStatus == shippingStatus : null;

                int totalItems = await _unitOfWork.ShippingDetails.CountAsync(filterExpression);
                var shippingDetails = await _unitOfWork.ShippingDetails.GetAll<ShippingDetailsResponse>(
                    filter: filterExpression,
                    orderBy: shipping => shipping.OrderBy(entity => entity.ShippingDate),
                    skip: ((page ?? 1) - 1) * Pagination.DefaultItemsPerPage,
                    take: Pagination.DefaultItemsPerPage);

                var meta = new Meta(totalItems, Pagination.DefaultItemsPerPage, page ?? 1);
                
                return new APIResponse<IEnumerable<ShippingDetailsResponse>>(shippingDetails, meta);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<APIResponse<ShippingDetailsResponse>> UpdateShippingDetailsAsync(
            int shippingId, UpdateShippingDetailsRequest shippingDetailsRequest)
        {
            try
            {
                ShippingDetails shippingDetails = await _unitOfWork.ShippingDetails.GetFirstOrDefault(
                    shipping => shipping.Id == shippingId)
                    ?? throw new Exception("Shipping details cannot be found");

                _mapper.Map(shippingDetailsRequest, shippingDetails);
                await _unitOfWork.SaveAsync();

                var response = _mapper.Map<ShippingDetailsResponse>(shippingDetails);

                return new APIResponse<ShippingDetailsResponse>(response);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
