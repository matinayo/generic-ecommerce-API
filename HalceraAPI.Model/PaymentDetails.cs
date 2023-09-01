using HalceraAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "decimal(10,4)")]
        public decimal AmountPaid { get; set; }
        [Required]
        public Currency? Currency { get; set; }
        /// <summary>
        /// Total Amount to be paid
        /// </summary>
        [Column(TypeName = "decimal(10,4)")]
        public decimal? TotalAmount { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; } = DateTime.UtcNow;
        public DateTime? PaymentDueDate { get; set; }
        public string? TransactionId { get; set; }
    }
}
