using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.ShoppingCart
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
        [Required]
        public required string Reference { get; set; }
        public PaymentProvider PaymentProvider { get; set; }
        public string? Channel { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentDueDate { get; set; }
    }
}
