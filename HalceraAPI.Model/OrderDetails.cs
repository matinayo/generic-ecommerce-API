using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Customer order on each product
    /// </summary>
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PurchaseDetailsId { get; set; }
        [ForeignKey(nameof(PurchaseDetailsId))]
        public PurchaseDetails? PurchaseDetails { get; set; }

        [Required]
        public string? OrderHeaderId { get; set; }
        [ForeignKey(nameof(OrderHeaderId))]
        public OrderHeader? OrderHeader { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}
