using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    public class Price
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,4)")]
        public decimal? Amount { get; set; }

        [Required]
        public Currency? Currency { get; set; }

        [Column(TypeName = "decimal(10,4)")]
        public decimal? DiscountAmount { get; set; }

        public int? CompositionId { get; set; }

        [ForeignKey(nameof(CompositionId))]
        public Composition? Composition { get; set; }
    }
}
