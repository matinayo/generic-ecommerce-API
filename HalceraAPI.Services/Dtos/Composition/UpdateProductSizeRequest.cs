namespace HalceraAPI.Services.Dtos.Composition
{
    public record UpdateProductSizeRequest
    {
        public int? Id { get; init; }

        public string? Size { get; init; }

        public int? Quantity { get; init; }
    }
}
