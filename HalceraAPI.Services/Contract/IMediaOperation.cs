using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Media;

namespace HalceraAPI.Services.Contract
{
    public interface IMediaOperation
    {
        void UpdateMediaCollection(IEnumerable<UpdateMediaRequest>? mediaCollection, ICollection<Media>? mediaCollectionFromDb);

        Task DeleteMediaCollection(int? categoryId, int? productId);
    }
}
