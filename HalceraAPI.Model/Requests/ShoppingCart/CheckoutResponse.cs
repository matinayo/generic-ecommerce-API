using HalceraAPI.Models.Enums;

namespace HalceraAPI.Models.Requests.ShoppingCart
{
    public class CheckoutResponse
    {
        public int Id { get; set; }
        public string? OrderReferenceId { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
