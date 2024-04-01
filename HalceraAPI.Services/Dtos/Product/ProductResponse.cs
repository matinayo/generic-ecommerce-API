using HalceraAPI.Services.Dtos.Category;
using HalceraAPI.Services.Dtos.Media;
using HalceraAPI.Services.Dtos.Price;

namespace HalceraAPI.Services.Dtos.Product
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? Quantity { get; set; }
        /// <summary>
        /// if product is active
        /// </summary>
        public bool Active { get; set; }
        /// <summary>
        /// Featured Product
        /// </summary>
        public bool? Featured { get; set; }

        public ICollection<PriceResponse>? Prices { get; set; }

        /// <summary>
        /// Product Medias
        /// </summary>
        public ICollection<MediaResponse>? MediaCollection { get; set; }

        /// <summary>
        /// Product Categories
        /// </summary>
        public ICollection<CategoryLabelResponse>? Categories { get; set; }
    }
}
