using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Product Category class
    /// </summary>
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Field has a minimum length of '2' and maximum length of '10'", MinimumLength = 2)]
        public string? Title { get; set; }
        /// <summary>
        /// Category Medias
        /// </summary>
        public ICollection<Media>? MediaCollection { get; set; }

        public bool Active { get; set; } = true;

        public bool? Featured { get; set; }

        public DateTime? DateAdded { get; set; } = DateTime.UtcNow;

        public DateTime? DateLastModified { get; set; }
    }
}
