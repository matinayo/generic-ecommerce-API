using HalceraAPI.Services.Dtos.Media;
using HalceraAPI.Services.Dtos.Price;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Composition
{
    public class UpdateCompositionRequest
    {
        public int? Id { get; set; }

        [StringLength(20, ErrorMessage = "Field has a maximum length of '20'")]
        public string? ColorName { get; set; }

        public string? ColorCode { get; set; }

        public ICollection<UpdateProductSizeRequest>? Sizes { get; set; }

        public ICollection<UpdatePriceRequest>? Prices { get; set; }

        public ICollection<UpdateMediaRequest>? MediaCollection { get; set; }
    }
}
