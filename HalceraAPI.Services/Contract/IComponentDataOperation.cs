using HalceraAPI.Models;
using HalceraAPI.Services.Dtos.Composition.ComponentData;

namespace HalceraAPI.Services.Contract
{
    public interface IComponentDataOperation
    {
        void UpdateComponentData(
            IEnumerable<UpdateComponentDataRequest>? compositionDataRequests,
            ICollection<ComponentData>? existingCompositionDataFromDb);

        Task DeleteProductComponents(int productId);

        Task DeleteComponentDataByComponentIdAsync(int productId, int componentId);
    }
}
