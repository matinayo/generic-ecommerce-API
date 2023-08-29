﻿using HalceraAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Info on amount paid by customer on each order
    /// </summary>
    public class PurchaseDetails
    {
        [Key]
        public int Id { get; set; }
        public double? ProductAmountAtPurchase { get; set; }
        /// <summary>
        /// indicate if product price was at a discounted rate
        /// </summary>
        public double? DiscountAmount { get; set; }
        [Required]
        public Currency? Currency { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        public string? ApplicationUserId { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
