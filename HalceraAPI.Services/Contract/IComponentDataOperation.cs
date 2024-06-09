using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.Composition.ComponentData;

namespace HalceraAPI.Services.Contract
{
    public interface IComponentDataOperation
    {
        /// <summary>
        /// Update Product Composition Data
        /// </summary>
        void UpdateComponentData(
            IEnumerable<UpdateComponentDataRequest>? compositionDataRequests,
            ICollection<ComponentData>? existingCompositionDataFromDb);

        /// <summary>
        /// Delete range of Composition Data
        /// </summary>
        /// <param name="compositionsId">List of Composition Id to be deleted</param>
        /// <returns>Returns false if nothing was deleted otherwise true</returns>
        Task DeleteCompositionDataCollectionAsync(IEnumerable<int> compositionIdCollection);

        Task DeleteCompositionDataFromProductCompositionAsync(int productId, int compositionId, int compositionDataId);
    }
}
