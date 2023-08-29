using HalceraAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.ShoppingCart
{
    public class PaymentDetailsRequest
    {
        [Required]
        public decimal AmountPaid { get; set; }
        [Required]
        public Currency Currency { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        [Required]
        public string? TransactionId { get; set; }
    }
}
