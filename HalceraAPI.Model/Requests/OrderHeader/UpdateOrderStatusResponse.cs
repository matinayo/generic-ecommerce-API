using HalceraAPI.Models.Enums;

namespace HalceraAPI.Models.Requests.OrderHeader
{
    public record UpdateOrderStatusResponse
    {
        public string? OrderId { get; init; }
        public OrderStatus OrderStatus { get; init; }
    }
}
