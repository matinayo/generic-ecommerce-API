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
        void UpdateComposition(IEnumerable<UpdateCompositionRequest>? compositionCollection, ICollection<Composition>? existingCompositionsfromDb);
        
        Task<bool> DeleteProductCompositions(int productId);

        Task DeleteProductCompositionByCompositionId(int productId, int compositionId);
    }
}
