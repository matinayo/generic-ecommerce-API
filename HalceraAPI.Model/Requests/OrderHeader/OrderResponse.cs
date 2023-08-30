using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.PaymentDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Requests.OrderHeader
{
    /// <summary>
    /// Admin Order Header response
    /// </summary>
    public class OrderResponse
    {
        public string? Id { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public DateTime? OrderDate { get; set; }

        public PaymentDetailsResponse? PaymentDetails { get; set; }

    }
}
