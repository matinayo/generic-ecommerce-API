using HalceraAPI.Models;
using HalceraAPI.Models.Requests.Media;

namespace HalceraAPI.Services.Contract
{
    public interface IMediaOperation
    {
        Task<ICollection<Media>?> UpdateMediaCollection(IEnumerable<UpdateMediaRequest>? mediaCollection);
    }
}
