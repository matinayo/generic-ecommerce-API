using HalceraAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Order and User data
    /// </summary>
    public class OrderHeader
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public OrderStatus OrderStatus { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;

        public int? PaymentDetailId { get; set; }
        [ForeignKey(nameof(PaymentDetailId))]
        public PaymentDetails? PaymentDetails { get; set; }

        public int? ShippingDetailsId { get; set; }
        [ForeignKey(nameof(ShippingDetailsId))]
        public ShippingDetails? ShippingDetails { get; set; }

        /// <summary>
        /// Corresponding order details
        /// </summary>
        public ICollection<OrderDetails>? OrderDetails { get; set; }

        [Required]
        public string? ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }

    }
}
