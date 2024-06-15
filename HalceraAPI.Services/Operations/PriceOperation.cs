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

        public async Task DeletePriceFromCompositionByPriceIdAsync(int compositionId, int priceId)
        {
            Price priceToDelete = await _unitOfWork.Price.GetFirstOrDefault(
                price => price.Id == priceId
                && price.CompositionId == compositionId)
                ?? throw new Exception("No price available for this composition.");

            _unitOfWork.Price.Remove(priceToDelete);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeletePricesByListOfCompositionIdAsync(List<int> compositionIds)
        {
            IEnumerable<Price>? priceCollection = await _unitOfWork.Price.GetAll(
                                                                    price => price.CompositionId != null
                                                                    && compositionIds.Contains(price.CompositionId ?? 0));

            if (priceCollection is not null && priceCollection.Any())
            {
                _unitOfWork.Price.RemoveRange(priceCollection);
            }
        }

        public async Task ResetDiscountOfCompositionPriceByPriceIdAsync(int compositionId, int priceId)
        {
            Price price = await _unitOfWork.Price.GetFirstOrDefault(
                price => price.Id == priceId
                && price.CompositionId == compositionId)
                ?? throw new Exception("No price available for this composition.");

            price.DiscountAmount = null;
            await _unitOfWork.SaveAsync();
        }

        public void UpdatePrice(
            IEnumerable<UpdatePriceRequest>? priceCollection,
            ICollection<Price>? existingPriceFromDb)
        {
            if (priceCollection is not null && priceCollection.Any())
            {
                existingPriceFromDb ??= new List<Price>();

                var tempPrices = priceCollection.Select(u => u.Currency).ToList();
                tempPrices.AddRange(existingPriceFromDb.Select(u => u.Currency));
                foreach (var priceRequest in priceCollection)
                {
                    Price? existingPrice = existingPriceFromDb?.FirstOrDefault(em => em.Id == priceRequest.Id);
                    if (existingPrice != null)
                    {
                        var samePrice = priceCollection.FirstOrDefault(u => u.Currency == existingPrice.Currency);
                        if (samePrice != null && samePrice.Id != existingPrice.Id)
                        {
                            throw new Exception($"Duplicate currency type: {samePrice.Currency?.ToString()} specified");
                        }
                        _mapper.Map(priceRequest, existingPrice);
                    }
                    else
                    {
                        int noOfCurrentCurrency = tempPrices.Where(u => u == priceRequest.Currency).Count();
                        if (noOfCurrentCurrency > 1)
                        {
                            throw new Exception($"Duplicate currency type: {priceRequest.Currency?.ToString()} specified");
                        }

                        existingPrice = _mapper.Map<Price>(priceRequest);

                        existingPriceFromDb?.Add(existingPrice);
                    }
                }
            }
        }

    }
}
