using HalceraAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Order Payment Details
    /// </summary>
    public class PaymentDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double? AmountPaid { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public string? TransactionId { get; set; }
    }
}
