using HalceraAPI.Services.Dtos.Category;
using HalceraAPI.Services.Dtos.Composition;
using HalceraAPI.Services.Dtos.Media;
using HalceraAPI.Services.Dtos.Price;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Product
{
    public class UpdateProductRequest
    {
        [StringLength(20, ErrorMessage = "Field has a minimum length of '2' and maximum length of '20'", MinimumLength = 2)]
        public string? Title { get; set; }

        [StringLength(100, ErrorMessage = "Field has a minimum length of 10 and maximum length of '100'", MinimumLength = 10)]
        public string? Description { get; set; }
        public int? Quantity { get; set; }

        /// <summary>
        /// if product is active
        /// </summary>
        public bool? Active { get; set; }

        /// <summary>
        /// Featured Product
        /// </summary>
        public bool? Featured { get; set; }

        public ICollection<UpdatePriceRequest>? Prices { get; set; }

        /// <summary>
        /// Product Medias
        /// </summary>
        public ICollection<UpdateMediaRequest>? MediaCollection { get; set; }

        /// <summary>
        /// Product Composition
        /// </summary>
        public ICollection<UpdateCompositionRequest>? ProductCompositions { get; set; }

        /// <summary>
        /// Product Categories
        /// </summary>
        public ICollection<ProductCategoryRequest>? Categories { get; set; }
    }
}
