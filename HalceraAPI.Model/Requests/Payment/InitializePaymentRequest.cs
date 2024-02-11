using HalceraAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.Payment
{
    public record InitializePaymentRequest
    {
        [Required]
        public string Email { get; init; } = string.Empty;
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public PaymentProvider PaymentProvider { get; set; }
    }
}
