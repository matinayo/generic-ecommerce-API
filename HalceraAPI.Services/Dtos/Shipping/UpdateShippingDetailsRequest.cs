using HalceraAPI.Common.Enums;

namespace HalceraAPI.Services.Dtos.Shipping
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
