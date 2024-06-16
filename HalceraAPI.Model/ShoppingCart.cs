using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Range(1, 10000, ErrorMessage = "Enter a range between 1 and 10000")]
        public int Quantity { get; set; } = 1;

        public int CompositionId { get; set; }

        public int ProductSizeId { get; set; }
        
        public string? ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }

        [ForeignKey(nameof(CompositionId))]
        public Composition? Composition { get; set; }

        [ForeignKey(nameof(ProductSizeId))]
        public ProductSize? ProductSize { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}
