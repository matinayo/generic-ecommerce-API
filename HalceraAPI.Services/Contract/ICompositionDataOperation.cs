using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.Composition.CompositionData;

namespace HalceraAPI.Services.Contract
{
    public interface ICompositionDataOperation
    {
        /// <summary>
        /// Update Product Composition Data
        /// </summary>
        void UpdateCompositionData(
            IEnumerable<UpdateCompositionDataRequest>? compositionDataRequests,
            ICollection<CompositionData>? existingCompositionDataFromDb);

        /// <summary>
        /// Delete range of Composition Data
        /// </summary>
        /// <param name="compositionsId">List of Composition Id to be deleted</param>
        /// <returns>Returns false if nothing was deleted otherwise true</returns>
        Task DeleteCompositionDataCollectionAsync(IEnumerable<int> compositionIdCollection);

        Task DeleteCompositionDataFromProductCompositionAsync(int productId, int compositionId, int compositionDataId);
    }
}
