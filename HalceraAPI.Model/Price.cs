using HalceraAPI.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models
{
    /// <summary>
    /// Pricing per currency
    /// </summary>
    public class Price
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double? Amount { get; set; }
        [Required]
        public Currency? Currency { get; set; }

        /// <summary>
        /// indicate if price is discounted
        /// </summary>
        public double? DiscountAmount { get; set; }

        public int? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }
    }

}
