using HalceraAPI.Models.Requests.OrderHeader.CustomerResponse.CustomerOrderDetails;
using HalceraAPI.Models.Requests.OrderHeader.CustomerResponse.PurchaseDetails;

namespace HalceraAPI.Models.Requests.OrderHeader.CustomerResponse
{
    public class OrderDetailsResponse
    {
        // Purchase Details
        public PurchaseDetailsSummaryResponse? PurchaseDetails { get; set; }
        // Product Summary
        public ProductSummaryResponse? Product { get; set; }
    }
}
