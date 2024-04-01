using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Info on amount paid by customer on each order
    /// </summary>
    public class PurchaseDetails
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "decimal(10,4)")]
        public decimal? ProductAmountAtPurchase { get; set; }
        /// <summary>
        /// indicate if product price was at a discounted rate
        /// </summary>
        [Column(TypeName = "decimal(10,4)")]
        public decimal? DiscountAmount { get; set; }
        [Required]
        public Currency? Currency { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public string? ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
