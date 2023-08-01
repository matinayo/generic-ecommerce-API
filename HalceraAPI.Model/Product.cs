using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Product model class
    /// </summary>
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "Field has a minimum length of '2' and maximum length of '20'", MinimumLength = 2)]
        public string? Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Field has a minimum length of 10 and maximum length of '100'", MinimumLength = 10)]
        public string? Description { get; set; }

        /// <summary>
        /// Product quantity available in stock
        /// </summary>
        public int? Quantity { get; set; } = 1;

        /// <summary>
        /// if product is active
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Featured Product
        /// </summary>
        public bool? Featured { get; set; }

        [Required]
        public ICollection<Price>? Prices { get; set; }
        /// <summary>
        /// Product Medias
        /// </summary>
        public ICollection<Media>? MediaCollection { get; set; }

        /// <summary>
        /// Product Ratings
        /// </summary>
        // public ICollection<Rating>? ProductRatings { get; set; }

        /// <summary>
        /// Product Composition
        /// </summary>
        public ICollection<Composition>? ProductCompositions { get; set; }

        /// <summary>
        /// Product Categories
        /// </summary>
        public ICollection<Category>? Categories { get; set; }

        //public DateTime? DateAdded { get; set; } = DateTime.Now;

        //public DateTime? DateLastModified { get; set; }
    }
}
