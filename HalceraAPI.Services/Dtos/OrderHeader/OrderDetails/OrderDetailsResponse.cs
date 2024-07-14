using HalceraAPI.Services.Dtos.OrderHeader.OrderDetails;

namespace HalceraAPI.Services.Dtos.OrderHeader.CustomerResponse
{
    public class OrderDetailsResponse
    {
        public int Id { get; set; }
        public string? Reference { get; set; }
        public PurchaseDetailsSummaryResponse? PurchaseDetails { get; set; }
        public ProductSummaryResponse? ProductReference { get; set; }

        public ProductSummaryResponse? Product { get; set; }
        public OrderProductSizeResponse? OrderProductSize { get; set; }
        public OrderCompositionResponse? OrderComposition { get; set; }
    }
}
