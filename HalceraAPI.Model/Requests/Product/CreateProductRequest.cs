using HalceraAPI.Models.Requests.Category;
using HalceraAPI.Models.Requests.Composition;
using HalceraAPI.Models.Requests.Media;
using HalceraAPI.Models.Requests.Price;
using HalceraAPI.Models.Requests.Rating;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.Product
{
    public record CreateProductRequest
    {
        [Required]
        [StringLength(20, ErrorMessage = "Field has a minimum length of '2' and maximum length of '20'", MinimumLength = 2)]
        public string? Title { get; init; }

        [Required]
        [StringLength(100, ErrorMessage = "Field has a minimum length of 10 and maximum length of '100'", MinimumLength = 10)]
        public string? Description { get; init; }

        public int? Quantity { get; init; }

        /// <summary>
        /// Indicates if a product is active
        /// </summary>
        public bool Active { get; init; } = true;

        /// <summary>
        /// Featured Product
        /// </summary>
        public bool? Featured { get; init; }

        /// <summary>
        /// Define Price for Product
        /// </summary>
        [Required]
        public ICollection<CreatePriceRequest>? Prices { get; init; }
        
        /// <summary>
        /// Define Product Medias
        /// </summary>
        public ICollection<CreateMediaRequest>? MediaCollection { get; init; }

        /// <summary>
        /// Define Product Composition
        /// </summary>
        public ICollection<CreateCompositionRequest>? ProductCompositions { get; init; }

        /// <summary>
        /// Product Categories
        /// </summary>
        public ICollection<ProductCategoryRequest>? Categories { get; init; }
    }
}
