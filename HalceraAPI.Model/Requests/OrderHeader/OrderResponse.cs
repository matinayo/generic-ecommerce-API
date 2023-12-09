using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader.CustomerResponse;
using HalceraAPI.Models.Requests.PaymentDetails;
using HalceraAPI.Models.Requests.Shipping;

namespace HalceraAPI.Models.Requests.OrderHeader
{
    public record OrderResponse
    {
        public string? Id { get; init; }
        public OrderStatus? OrderStatus { get; init; }

        public DateTime? OrderDate { get; init; }
        public DateTime? CancelledDate { get; set; }

        public PaymentDetailsResponse? PaymentDetails { get; init; }
        public ShippingDetailsResponse? ShippingDetails { get; init; }
        public ICollection<OrderDetailsResponse>? OrderDetails { get; init; }
        public CustomerDetailsResponse? ApplicationUser { get; init; }
    }
}
