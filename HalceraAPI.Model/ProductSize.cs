using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    public class ProductSize
    {
        public int Id { get; set; }
        [Required]
        public string? Size { get; set; }
        public int Quantity { get; set; } = 0;

        public int? CompositionId { get; set; }
        [ForeignKey(nameof(CompositionId))]
        public Composition? Composition { get; set; }
    }
}
