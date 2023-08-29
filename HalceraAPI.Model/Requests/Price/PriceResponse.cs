﻿using HalceraAPI.Models.Enums;

namespace HalceraAPI.Models.Requests.Price
{
    /// <summary>
    /// Price Data Response
    /// </summary>
    public class PriceResponse
    {
        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public Currency? Currency { get; set; }

        /// <summary>
        /// indicate if price is discounted
        /// </summary>
        public decimal? DiscountAmount { get; set; }
    }
}
