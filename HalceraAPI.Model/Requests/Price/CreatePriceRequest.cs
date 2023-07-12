using HalceraAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.Price
{
    /// <summary>
    /// Create Price Request
    /// </summary>
    public class CreatePriceRequest
    {
        [Required]
        public double? Amount { get; set; }
        [Required]
        public Currency? Currency { get; set; }

        /// <summary>
        /// indicate if price is discounted
        /// </summary>
        public double? DiscountAmount { get; set; }
    }
}
