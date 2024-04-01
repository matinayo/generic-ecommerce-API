using HalceraAPI.Common.Enums;

namespace HalceraAPI.Services.Dtos.OrderHeader
{
    public record UpdateOrderStatusResponse
    {
        public string? OrderId { get; init; }
        public OrderStatus OrderStatus { get; init; }
    }
}
