using HalceraAPI.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Dtos.OrderHeader.OrderDetails
{
    public class PurchaseDetailsSummaryResponse
    {
        public int Id { get; set; }
        public decimal? ProductAmountAtPurchase { get; set; }
        public decimal? DiscountAmount { get; set; }
        public Currency? Currency { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
