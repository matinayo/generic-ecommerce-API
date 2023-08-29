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
        public decimal AmountPaid { get; set; }
        [Required]
        public Currency? Currency { get; set; }
        /// <summary>
        /// Total Amount to be paid
        /// </summary>
        public decimal? TotalAmount { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; } = DateTime.UtcNow;
        public DateTime? PaymentDueDate { get; set; }
        public string? TransactionId { get; set; }
    }
}
