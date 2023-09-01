using HalceraAPI.Models.Enums;

namespace HalceraAPI.Models.Requests.ShoppingCart
{
    public class CheckoutResponse
    {
        public string? Id { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
