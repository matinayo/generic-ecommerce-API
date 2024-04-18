using HalceraAPI.Services.Dtos.Media;
using HalceraAPI.Services.Dtos.Price;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Composition
{
    public class CreateCompositionRequest
    {
        [Required]
        [StringLength(20, ErrorMessage = "Field has a maximum length of '20'")]
        public required string ColorName { get; init; }

        [Required]
        public required string ColorCode { get; init; }

        public ICollection<ProductSizeRequest>? Sizes { get; init; }

        [Required]
        public ICollection<CreatePriceRequest>? Prices { get; init; }

        public ICollection<CreateMediaRequest>? MediaCollection { get; init; }
    }
}
