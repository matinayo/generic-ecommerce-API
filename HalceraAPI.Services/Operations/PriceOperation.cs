using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Dtos.Price;

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

        public async Task DeletePriceFromProductByPriceIdAsync(int productId, int priceId)
        {
            try
            {
                //Price priceToDelete = await _unitOfWork.Price.GetFirstOrDefault(
                //    price => price.Id == priceId
                //    && price.ProductId == productId)
                //    ?? throw new Exception("No price available for this product");

                //_unitOfWork.Price.Remove(priceToDelete);
                //await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteProductPricesAsync(int productId)
        {
            try
            {
                //IEnumerable<Price>? productPrices = await _unitOfWork.Price.GetAll(price => price.ProductId == productId);
                //if (productPrices is not null && productPrices.Any())
                //{
                //    _unitOfWork.Price.RemoveRange(productPrices);
                //    await _unitOfWork.SaveAsync();
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ResetDiscountOfProductPriceByPriceIdAsync(int productId, int priceId)
        {
            try
            {
                //Price price = await _unitOfWork.Price.GetFirstOrDefault(
                //    price => price.Id == priceId
                //    && price.ProductId == productId)
                //    ?? throw new Exception("No price available for this product");

                //price.DiscountAmount = null;
                //await _unitOfWork.SaveAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePrice(
            IEnumerable<UpdatePriceRequest>? priceCollection,
            ICollection<Price>? existingPriceFromDb)
        {
            if (priceCollection is not null && priceCollection.Any())
            {
                existingPriceFromDb ??= new List<Price>();
                foreach (var priceRequest in priceCollection)
                {
                    Price? existingPrice = existingPriceFromDb?.FirstOrDefault(em => em.Id == priceRequest.Id);
                    if (existingPrice != null)
                    {
                        _mapper.Map(priceRequest, existingPrice);
                    }
                    else
                    {
                        existingPrice = _mapper.Map<Price>(priceRequest);
                        existingPriceFromDb?.Add(existingPrice);
                    }
                }
            }
        }
    }
}
