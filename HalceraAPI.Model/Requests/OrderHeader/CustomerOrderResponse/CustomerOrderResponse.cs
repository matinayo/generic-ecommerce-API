using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.PaymentDetails;
using HalceraAPI.Models.Requests.Shipping;

namespace HalceraAPI.Models.Requests.OrderHeader.CustomerResponse
{
    public record CustomerOrderResponse
    {
        public string? Id { get; init; }
        public OrderStatus? OrderStatus { get; init; }

        public DateTime? OrderDate { get; init; }

        public CustomerPaymentResponse? PaymentDetails { get; init; }
        public ShippingDetailsResponse? ShippingDetails { get; init; }
        public ICollection<CustomerOrderDetailsResponse>? OrderDetails { get; init; }
    }
}
