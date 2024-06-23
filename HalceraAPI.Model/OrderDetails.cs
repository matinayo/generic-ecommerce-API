using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }

        public string? Reference { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public int CompositionId { get; set; }

        [Required]
        public int ProductSizeId { get; set; }

        [Required]
        public int PurchaseDetailsId { get; set; }

        [Required]
        public int ProductReferenceId { get; set; }

        [Required]
        public string? OrderHeaderId { get; set; }

        [ForeignKey(nameof(PurchaseDetailsId))]
        public PurchaseDetails? PurchaseDetails { get; set; }

        [ForeignKey(nameof(OrderHeaderId))]
        public OrderHeader? OrderHeader { get; set; }

        [ForeignKey(nameof(ProductReferenceId))]
        public Product? Product { get; set; }

        // Add OrderProduct and reference to OrderProduct

        [ForeignKey(nameof(ProductSizeId))]
        public OrderProductSize? ProductSize { get; set; }

        [ForeignKey(nameof(CompositionId))]
        public OrderComposition? Composition { get; set; }
    }
}
