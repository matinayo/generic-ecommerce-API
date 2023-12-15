﻿using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Models.Requests.Price;

namespace HalceraAPI.Services.Contract
{
    public interface IPriceOperation
    {
        void UpdatePrice(IEnumerable<UpdatePriceRequest>? priceCollection, ICollection<Price>? pricesFromDb);
        Task DeleteProductPricesAsync(int productId);
        
    }
}
