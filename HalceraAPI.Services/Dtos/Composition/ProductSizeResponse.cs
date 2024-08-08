namespace HalceraAPI.Services.Dtos.Composition
{
    public record ProductSizeResponse
    {
        public int Id { get; init; }

        public string? Size { get; init; }

        public int Quantity { get; init; }
    }
}
