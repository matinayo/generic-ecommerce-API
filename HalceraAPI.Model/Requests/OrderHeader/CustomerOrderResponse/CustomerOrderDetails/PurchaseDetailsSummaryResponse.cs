using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Requests.OrderHeader.CustomerResponse.CustomerOrderDetails
{
    /// <summary>
    /// Product purchase summary
    /// </summary>
    public class PurchaseDetailsSummaryResponse
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }
}
