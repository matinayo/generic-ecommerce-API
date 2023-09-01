using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.PaymentDetails;

namespace HalceraAPI.Models.Requests.OrderHeader.CustomerResponse
{
    public class CustomerOrderResponse
    {
        public string? Id { get; set; }
        public OrderStatus? OrderStatus { get; set; }

        public DateTime? OrderDate { get; set; }

        public CustomerPaymentResponse? PaymentDetails { get; set; }
        public ICollection<CustomerOrderDetailsResponse>? OrderDetails { get; set; }
    }
}
