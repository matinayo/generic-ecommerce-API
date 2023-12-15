using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.APIResponse;
using HalceraAPI.Models.Requests.Shipping;

namespace HalceraAPI.Services.Contract
{
    public interface IShippingOperation
    {
        Task<APIResponse<ShippingDetailsResponse>> UpdateShippingDetailsAsync(
            int shippingId, UpdateShippingDetailsRequest shippingDetailsRequest);

        Task<APIResponse<IEnumerable<ShippingDetailsResponse>>> GetAllShippingRequestsAsync(ShippingStatus? shippingStatus, int? page);
    }
}
