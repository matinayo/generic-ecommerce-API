﻿using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.Media;
using HalceraAPI.Services.Dtos.Price;

namespace HalceraAPI.Services.Contract
{
    public interface IPriceOperation
    {
        Task DeletePricesByListOfCompositionIdAsync(List<int> compositionIds);
        void UpdatePrice(IEnumerable<UpdatePriceRequest>? priceCollection, ICollection<Price>? pricesFromDb);
        Task DeletePriceFromCompositionByPriceIdAsync(int compositionId, int priceId);
        Task ResetDiscountOfCompositionPriceByPriceIdAsync(int compositionId, int priceId);
    }
}
