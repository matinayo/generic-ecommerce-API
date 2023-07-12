using HalceraAPI.Models.Requests.Category;
using HalceraAPI.Models.Requests.Composition;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Models.Requests.Price;
using HalceraAPI.Models.Requests.Rating;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.Product
{
    public class CreateProductRequest
    {
        [Required]
        [StringLength(20, ErrorMessage = "Field has a minimum length of '2' and maximum length of '20'", MinimumLength = 2)]
        public string? Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Field has a minimum length of 10 and maximum length of '100'", MinimumLength = 10)]
        public string? Description { get; set; }

        /// <summary>
        /// Indicates if a product is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Featured Product
        /// </summary>
        public bool? IsFeatured { get; set; }

        [Required]
        public ICollection<CreatePriceRequest>? Prices { get; set; }
        
        /// <summary>
        /// Product Medias
        /// </summary>
        public ICollection<CreateMediaRequest>? MediaCollection { get; set; }

        /// <summary>
        /// Product Ratings
        /// </summary>
        public ICollection<CreateRatingRequest>? ProductRatings { get; set; }

        /// <summary>
        /// Product Composition
        /// </summary>
        public ICollection<CreateCompositionRequest>? ProductCompositions { get; set; }

        /// <summary>
        /// Product Categories
        /// </summary>
        public ICollection<CreateCategoryRequest>? Categories { get; set; }
    }
}
