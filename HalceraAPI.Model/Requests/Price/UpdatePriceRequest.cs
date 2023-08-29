using HalceraAPI.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Requests.Price
{
    /// <summary>
    /// Update Price Request
    /// </summary>
    public class UpdatePriceRequest
    {
        public int? Id { get; set; }
        public decimal? Amount { get; set; }
        public Currency? Currency { get; set; }

        /// <summary>
        /// indicate if price is discounted
        /// </summary>
        public decimal? DiscountAmount { get; set; }
    }
}
