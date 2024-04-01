using Enums = HalceraAPI.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.ShoppingCart
{
    public class ShoppingCartRequest
    {
        [Range(1, 100)]
        public int? Quantity { get; set; } = 1;
        public Enums.Currency Currency { get; set; } = Enums.Currency.NGN;
    }
}
