using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Media;

namespace HalceraAPI.Services.Contract
{
    public interface IMediaOperation
    {
        void UpdateMediaCollection(IEnumerable<UpdateMediaRequest>? mediaCollection, ICollection<Media>? mediaCollectionFromDb);
        /// <summary>
        /// Delete range of Media collection
        /// </summary>
        /// <param name="categoryId">Category Id media to be deleted</param>
        /// <param name="productId">Product Id media to be deleted</param>
        /// <returns>Returns false if nothing was deleted</returns>
        Task<bool> DeleteMediaCollection(int? categoryId, int? productId);
    }
}
