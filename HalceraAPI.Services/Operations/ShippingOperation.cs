using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.Shipping;
using HalceraAPI.Services.Contract;

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

        public async Task<IEnumerable<ShippingDetailsResponse>> GetAllShippingRequestsAsync(ShippingStatus? shippingStatus)
        {
            try
            {
                return await _unitOfWork.ShippingDetails.GetAll<ShippingDetailsResponse>(
                    filter: shippingStatus != null ? shipping => shipping.ShippingStatus == shippingStatus : null,
                    orderBy: shipping => shipping.OrderBy(entity => entity.ShippingDate));

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ShippingDetailsResponse> UpdateShippingDetailsAsync(int shippingId, UpdateShippingDetailsRequest shippingDetailsRequest)
        {
            try
            {
                ShippingDetails shippingDetails = await _unitOfWork.ShippingDetails.GetFirstOrDefault(shipping => shipping.Id == shippingId)
                    ?? throw new Exception("Shipping details cannot be found");

                _mapper.Map(shippingDetailsRequest, shippingDetails);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<ShippingDetailsResponse>(shippingDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
