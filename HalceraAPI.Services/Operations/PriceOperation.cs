using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Price;
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
            try
            {
                IEnumerable<Price>? productPrices = await _unitOfWork.Price.GetAll(price => price.ProductId == productId);
                if (productPrices is not null && productPrices.Any())
                {
                    _unitOfWork.Price.RemoveRange(productPrices);
                    await _unitOfWork.SaveAsync();
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePrice(IEnumerable<UpdatePriceRequest>? priceCollection, ICollection<Price>? existingPriceFromDb)
        {
            try
            {
                if (priceCollection is not null && priceCollection.Any())
                {
                    existingPriceFromDb ??= new List<Price>();
                    foreach (var priceRequest in priceCollection)
                    {
                        // Find existing price with the same ID in the database
                        Price? existingPrice = existingPriceFromDb?.FirstOrDefault(em => em.Id == priceRequest.Id);
                        if (existingPrice != null)
                        {
                            // If the price already exists, update its properties
                            _mapper.Map(priceRequest, existingPrice);
                        }
                        else
                        {
                            // If the price does not exist, create a new price object and map the properties
                            Price newPrice = _mapper.Map<Price>(priceRequest);
                            existingPriceFromDb?.Add(newPrice);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
