using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.Shipping;

namespace HalceraAPI.Services.Contract
{
    public interface IShippingOperation
    {
        Task UpdateShippingDetailsAsync(int shippingId, UpdateShippingDetailsRequest shippingDetailsRequest);

        Task GetAllShippingRequests(ShippingStatus? shippingStatus);
    }
}
