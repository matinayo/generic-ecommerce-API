using HalceraAPI.Services.Dtos.BaseAddress;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.ShoppingCart
{
    public class CheckoutRequest
    {
        [Required]
        public required PaymentDetailsRequest PaymentDetailsRequest { get; set; }
        [Required]
        public required AddressRequest ShippingAddress { get; set; }
    }
}
