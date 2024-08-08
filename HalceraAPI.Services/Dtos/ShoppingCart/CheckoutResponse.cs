using HalceraAPI.Common.Enums;

namespace HalceraAPI.Services.Dtos.ShoppingCart
{
    public class CheckoutResponse
    {
        public string? Id { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
