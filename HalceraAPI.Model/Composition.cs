using HalceraAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Product Composition
    /// </summary>
    public class Composition
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Type of product composition
        /// </summary>
        [Required]
        public CompositionType? CompositionType { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Field has a maximum length of '10'")]
        public string? Name { get; set; }
        public ICollection<CompositionData>? CompositionDataCollection { get; set; }
    }
}
