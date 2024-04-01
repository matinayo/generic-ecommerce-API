using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
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
        public string? Reference { get; set; }
        public PaymentProvider? PaymentProvider { get; set; }
        public string? Channel { get; set; }
    }
}
