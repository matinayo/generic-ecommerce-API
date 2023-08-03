using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Models.Requests.Price;

namespace HalceraAPI.Services.Contract
{
    public interface IPriceOperation
    {
        /// <summary>
        /// Update Product Prices
        /// </summary>
        void UpdatePrice(IEnumerable<UpdatePriceRequest>? priceCollection, ICollection<Price>? pricesFromDb);
        /// <summary>
        /// Delete range of Composition collection
        /// </summary>
        /// <param name="productId">Product Id Composition to be deleted</param>
        /// <returns>Returns false if nothing was deleted otherwise true</returns>
        Task<bool> DeleteProductPrices(int productId);
    }
}
