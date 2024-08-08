using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Shipping
{
    public record UpdateShippingAddressRequest
    {
        [StringLength(80, ErrorMessage = "Field has a maximum length of '80'")]
        public string? StreetAddress { get; init; }
        [StringLength(40, ErrorMessage = "Field has a maximum length of '40'")]
        public string? City { get; init; }
        [StringLength(80, ErrorMessage = "Field has a maximum length of '80'")]
        public string? State { get; init; }
        public string? Country { get; init; }
        [StringLength(10, ErrorMessage = "Field has a maximum length of '10'")]
        public string? PostalCode { get; init; }
        public string? PhoneNumber { get; init; }
    }
}
