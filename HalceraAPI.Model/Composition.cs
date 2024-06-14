using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    public class Composition
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Field has a maximum length of '20'")]
        public required string ColorName { get; set; }

        [Required]
        public required string ColorCode { get; set; }

        [Required]
        public ICollection<ProductSize>? Sizes { get; set; }

        [Required]
        public ICollection<Price>? Prices { get; set; }

        public ICollection<Media>? MediaCollection { get; set; }

        public int? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

    }
}
