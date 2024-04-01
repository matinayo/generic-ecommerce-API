using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.Price
{
    /// <summary>
    /// Create Price Request
    /// </summary>
    public class CreatePriceRequest
    {
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public Currency? Currency { get; set; }

        /// <summary>
        /// indicate if price is discounted
        /// </summary>
        public decimal? DiscountAmount { get; set; }
    }
}
