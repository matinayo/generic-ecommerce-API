using HalceraAPI.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Requests.OrderHeader
{
    public record OrderOverviewResponse
    {
        public string? Id { get; init; }
        public OrderStatus? OrderStatus { get; init; }

        public DateTime? OrderDate { get; init; }
        public DateTime? CancelledDate { get; init; }
    }
}
