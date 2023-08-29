using HalceraAPI.Models.Requests.BaseAddress;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.ShoppingCart
{
    public class CheckoutRequest
    {
        // payment status and address details
        [Required]
        public required PaymentDetailsRequest PaymentDetailsRequest { get; set; }
        [Required]
        public required AddressRequest ShippingAddress { get; set; }
    }
}
