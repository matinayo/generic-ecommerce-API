using HalceraAPI.Services.Dtos.Media;
using HalceraAPI.Services.Dtos.Price;

namespace HalceraAPI.Services.Dtos.Composition
{
    public record CompositionResponse
    {
        public int Id { get; init; }

        public string? ColorName { get; init; }

        public string? ColorCode { get; init; }

        public ICollection<ProductSizeResponse>? Sizes { get; init; }

        public ICollection<PriceResponse>? Prices { get; init; }

        public ICollection<MediaResponse>? MediaCollection { get; init; }
    }
}
