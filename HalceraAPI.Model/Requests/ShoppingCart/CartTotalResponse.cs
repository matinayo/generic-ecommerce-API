using HalceraAPI.Models.Enums;

namespace HalceraAPI.Models.Requests.ShoppingCart
{
    public record CartTotalResponse
    {
        public decimal TotalAmount { get; init; }
        public Currency CurrencyToBePaidIn { get; init; }
    }
}
