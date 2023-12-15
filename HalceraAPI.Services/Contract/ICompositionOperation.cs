using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Composition;

namespace HalceraAPI.Services.Contract
{
    public interface ICompositionOperation
    {
        void UpdateComposition(IEnumerable<UpdateCompositionRequest>? compositionCollection, ICollection<Composition>? existingCompositionsfromDb);

        Task<bool> DeleteProductCompositions(int productId);

        Task DeleteCompositionFromProductByCompositionIdAsync(int productId, int compositionId);
    }
}
