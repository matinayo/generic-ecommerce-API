using Enums = HalceraAPI.Common.Enums;

namespace HalceraAPI.Services.Dtos.Shipping
{
    public record ShippingDetailsResponse
    {
        public int Id { get; init; }
        public string? TrackingNumber { get; init; }
        public string? Carrier { get; init; }
        public Enums.ShippingStatus? ShippingStatus { get; init; } = Enums.ShippingStatus.Pending;
        public DateTime? ShippingDate { get; init; }
        public DateTime? DateShipped { get; init; }
        public DateTime? CancelledDate { get; set; }
        public ShippingAddressResponse? ShippingAddress { get; init; }
    }
}
