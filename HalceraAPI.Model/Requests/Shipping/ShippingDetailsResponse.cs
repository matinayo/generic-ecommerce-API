using HalceraAPI.Models.Enums;

namespace HalceraAPI.Models.Requests.Shipping
{
    public record ShippingDetailsResponse
    {
        public int Id { get; init; }
        public string? TrackingNumber { get; init; }
        public string? Carrier { get; init; }
        public ShippingStatus? ShippingStatus { get; init; } = Enums.ShippingStatus.Pending;
        public DateTime? ShippingDate { get; init; }
        public DateTime? DateShipped { get; init; }
        public ShippingAddressResponse? ShippingAddress { get; init; }
    }
}
