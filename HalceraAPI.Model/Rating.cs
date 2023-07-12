using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Product Rating
    /// </summary>
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, 5)]
        public double? Rate { get; set; }
        [StringLength(100, ErrorMessage = "Comment has a minimum length of '10' and maximum length of '100'", MinimumLength = 10)]
        public string? Comment { get; set; }

        public int? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }
}