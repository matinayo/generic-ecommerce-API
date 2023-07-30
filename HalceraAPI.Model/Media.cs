using HalceraAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Media Content
    /// </summary>
    public class Media
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Content URL
        /// </summary>
        [Required]
        [StringLength(80)]
        public string? URL { get; set; }
        /// <summary>
        /// Content Type
        /// </summary>
        [Required]
        public MediaType Type { get; set; }
        /// <summary>
        /// Name or Caption of Media
        /// </summary>
        [StringLength(20)]
        public string? Name { get; set; }

        public int? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
    }
}
