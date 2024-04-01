using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models
{
    public class SMESalesReport
    {
        public int? SMESalesReportId { get; set; }
        public int BranchOfficeId { get; set; }
        public DateTime StartPeriod { get; set; }
        public DateTime EndPeriod { get; set; }

        public int? AppraisalCount { get; set; }
        public int? InstructionsCount { get; set; }
        public double? InstructionsFees { get; set; }
        public double? InstructionsTotalValueOfProperties { get; set; }
        public int? InstructionsWithdrawn { get; set; }
        public int? InstructionsApplicants { get; set; }

        public int? ViewingsCount { get; set; }
        public int? UniqueViewers { get; set; }

        public int? SalesCount { get; set; }
        public double? FeesFromSales { get; set; }
        public double? AverageSalePrice { get; set; }
        public double? TotalValueOfSale { get; set; }

        public int? FallThroughsCount { get; set; }
        public double? FallThroughValue { get; set; }

        public int? RejectionsCount { get; set; }
        public double? RejectionsValue { get; set; }

        public int? NetSalesCount { get; set; }
        public double? NetSalesFees { get; set; }

        public int? ExchangesCount { get; set; }
        public double? TotalFeesOnExchanges { get; set; }
        public double? AverageFeePerExchange { get; set; }
        public double? AverageFeePercentage { get; set; }
        public double? ExchangeTotalSalePrice { get; set; }

        public int? PipelineCount { get; set; }
        public double? PipelineTotalFees { get; set; }
        public double? PipelineAverageFee { get; set; }
        public double? PipelineTotalSalePrice { get; set; }

        public int? StockCount { get; set; }
        public double? StockTotalFees { get; set; }
        public double? StockAverageFee { get; set; }
        public double? StockTotalSalePrice { get; set; }
    }
}
