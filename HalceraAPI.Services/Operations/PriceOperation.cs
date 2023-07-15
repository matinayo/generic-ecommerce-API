using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Services.Contract;

namespace HalceraAPI.Services.Operations
{
    public class PriceOperation : IPriceOperation
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PriceOperation(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> DeleteProductPrices(int productId)
        {
            try {
                IEnumerable<Price>? productPrices = await _unitOfWork.Price.GetAll(price => price.ProductId == productId);
                if (productPrices is not null && productPrices.Any())
                {
                    _unitOfWork.Price.RemoveRange(productPrices);
                    await _unitOfWork.SaveAsync();
                }
                return true;
            } catch (Exception)
            {
                throw;
            }
        }

        public Task<ICollection<Media>?> UpdateComposition(IEnumerable<UpdateMediaRequest>? mediaCollection)
        {
            throw new NotImplementedException();
        }

    }
}
