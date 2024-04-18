using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    public class MaterialData
    {
        [Key]
        public int Id { get; set; }

        public CompositionType? CompositionType { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Data field has a maximum length of '20'")]
        public string? Data { get; set; }

        public int? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}
