using HalceraAPI.Common.Enums;

namespace HalceraAPI.Services.Dtos.Composition.ComponentData
{
    public record ComponentDataResponse
    {
        public int Id { get; init; }

        public ComponentType? CompositionType { get; init; }

        public string? Data { get; init; }
    }
}