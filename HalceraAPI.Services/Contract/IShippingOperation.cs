using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.Shipping;

namespace HalceraAPI.Services.Contract
{
    public interface IShippingOperation
    {
        Task<ShippingDetailsResponse> UpdateShippingDetailsAsync(int shippingId, UpdateShippingDetailsRequest shippingDetailsRequest);

        Task<IEnumerable<ShippingDetailsResponse>> GetAllShippingRequestsAsync(ShippingStatus? shippingStatus);
    }
}
