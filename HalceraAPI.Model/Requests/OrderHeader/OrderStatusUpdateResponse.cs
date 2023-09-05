using HalceraAPI.Models.Enums;

namespace HalceraAPI.Models.Requests.OrderHeader
{
    public record OrderStatusUpdateResponse
    {
        public string? OrderId { get; init; }
        public OrderStatus OrderStatus { get; init; }
    }
}
