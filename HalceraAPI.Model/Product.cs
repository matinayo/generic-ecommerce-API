using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Model
{
    /// <summary>
    /// Product model class
    /// </summary>
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(length: 20, ErrorMessage = "Title field has a maximum length of '20'")]
        [MinLength(length: 2, ErrorMessage = "Title field has a minimum length of '2'")]
        public string? Title { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Description field has a maximum length of '100'")]
        [MinLength(10, ErrorMessage = "Description field has a minimum length of '10'")]
        public string? Description { get; set; }

        [Required]
        public double Price { get; set; }
        public string? ImageURL { get; set; }
        public string? GlbModelURL { get; set; }
        //public string? VideoURL { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
    }
}
