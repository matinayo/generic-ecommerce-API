using HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace HalceraAPI.Services.Dtos.Price
{
    public record UpdatePriceRequest
    {
        public int? Id { get; init; }

        [Column(TypeName = "decimal(10,4)")]
        public decimal? Amount { get; init; }

        public Currency? Currency { get; init; }

        [Column(TypeName = "decimal(10,4)")]
        public decimal? DiscountAmount { get; init; }
    }
}
