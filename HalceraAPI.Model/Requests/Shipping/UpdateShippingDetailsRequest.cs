using HalceraAPI.Models.Enums;

namespace HalceraAPI.Models.Requests.Shipping
{
    public record UpdateShippingDetailsRequest
    {
        public string? TrackingNumber { get; init; }
        public string? Carrier { get; init; }
        public ShippingStatus? ShippingStatus { get; init; }
        public DateTime? ShippingDate { get; init; }
        public DateTime? DateShipped { get; init; }
        public DateTime? CancelledDate { get; init; }
        public UpdateShippingAddressRequest? ShippingAddress { get; init; }
    }
}
