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

        Task DeleteCompositionDataCollectionAsync(IEnumerable<int> compositionIdCollection);

        Task DeleteCompositionDataFromProductCompositionAsync(int productId, int compositionId, int compositionDataId);
        Task DeleteProductComponents(int productId);
    }
}
