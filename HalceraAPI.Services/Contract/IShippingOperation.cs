using HalceraAPI.Common.Enums;
using HalceraAPI.Services.Dtos.APIResponse;
using HalceraAPI.Services.Dtos.Shipping;

namespace HalceraAPI.Services.Contract
{
    public interface IShippingOperation
    {
        Task<APIResponse<ShippingDetailsResponse>> UpdateShippingDetailsAsync(
            int shippingId, UpdateShippingDetailsRequest shippingDetailsRequest);

        Task<APIResponse<IEnumerable<ShippingDetailsResponse>>> GetAllShippingRequestsAsync(ShippingStatus? shippingStatus, int? page);
    }
}
