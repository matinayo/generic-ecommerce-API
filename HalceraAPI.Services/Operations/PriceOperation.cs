using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Media;
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

        public async Task<ICollection<Price>?> UpdatePrice(IEnumerable<UpdatePriceRequest>? priceCollection)
        {
            try
            {
                if (priceCollection is not null && priceCollection.Any())
                {
                   IEnumerable<Price> existingPrices = await _unitOfWork.Price.GetAll(price => priceCollection.Select(u => u.Id).Contains(price.Id));
                    List<Price>? priceResponse = new();

                    foreach (var priceRequest in priceCollection)
                    {
                        // Find existing price with the same ID in the database
                        Price? existingPrice = existingPrices?.FirstOrDefault(em => em.Id == priceRequest.Id);

                        if (existingPrice != null)
                        {
                            // If the price already exists, update its properties
                            _mapper.Map(priceRequest, existingPrice);
                            priceResponse.Add(existingPrice);
                        }
                        else
                        {
                            // If the price does not exist, create a new price object and map the properties
                            Price newPrice = _mapper.Map<Price>(priceRequest);
                            priceResponse.Add(newPrice);
                        }
                    }
                    return priceResponse;
                }
                return null;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
