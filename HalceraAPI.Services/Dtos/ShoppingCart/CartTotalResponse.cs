using HalceraAPI.Common.Enums;

namespace HalceraAPI.Services.Dtos.ShoppingCart
{
    public record CartTotalResponse
    {
        public decimal TotalAmount { get; init; }
        public Currency CurrencyToBePaidIn { get; init; }
    }
}
