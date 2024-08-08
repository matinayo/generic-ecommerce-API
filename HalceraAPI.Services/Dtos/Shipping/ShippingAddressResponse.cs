namespace HalceraAPI.Services.Dtos.Shipping
{
    public record ShippingAddressResponse
    {
        public string? StreetAddress { get; init; }
        public string? City { get; init; }
        public string? State { get; init; }
        public string? Country { get; init; }
        public string? PostalCode { get; init; }
        public string? PhoneNumber { get; init; }
    }
}
