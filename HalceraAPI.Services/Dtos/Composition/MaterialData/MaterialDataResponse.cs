using HalceraAPI.Common.Enums;

namespace HalceraAPI.Services.Dtos.Composition.MaterialData
{
    public record MaterialDataResponse
    {
        public int Id { get; init; }

        public CompositionType? CompositionType { get; init; }

        public string? Data { get; init; }
    }
}