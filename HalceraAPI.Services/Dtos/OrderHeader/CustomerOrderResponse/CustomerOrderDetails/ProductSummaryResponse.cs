using HalceraAPI.Services.Dtos.Media;

namespace HalceraAPI.Services.Dtos.OrderHeader.CustomerResponse.PurchaseDetails
{
    /// <summary>
    /// Product Order Summary
    /// </summary>
    public class ProductSummaryResponse
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        /// <summary>
        /// Product Medias
        /// </summary>
        public ICollection<MediaResponse>? MediaCollection { get; set; }

    }
}
