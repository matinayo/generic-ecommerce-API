using HalceraAPI.Services.Dtos.OrderHeader.CustomerResponse.CustomerOrderDetails;
using HalceraAPI.Services.Dtos.OrderHeader.CustomerResponse.PurchaseDetails;

namespace HalceraAPI.Services.Dtos.OrderHeader.CustomerResponse
{
    public class OrderDetailsResponse
    {
        // Purchase Details
        public PurchaseDetailsSummaryResponse? PurchaseDetails { get; set; }
        // Product Summary
        public ProductSummaryResponse? Product { get; set; }
    }
}
