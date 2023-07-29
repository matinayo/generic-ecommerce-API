using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Composition;
using HalceraAPI.Models.Requests.Media;

namespace HalceraAPI.Services.Contract
{
    public interface ICompositionOperation
    {
        /// <summary>
        /// Update Product Composition
        /// </summary>
        Task UpdateComposition(int productId, IEnumerable<UpdateCompositionRequest>? compositionCollection, IEnumerable<Composition>? existingCompositionsfromDb);
        /// <summary>
        /// Delete range of Composition collection
        /// </summary>
        /// <param name="productId">Product Id Composition to be deleted</param>
        /// <returns>Returns false if nothing was deleted otherwise true</returns>
        Task<bool> DeleteProductCompositions(int productId);
    }
}
