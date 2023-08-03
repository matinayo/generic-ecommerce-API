using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Models.Requests.ShoppingCart
{
    public class ShoppingCartRequest
    {
        [Range(1, 100)]
        public int? Quantity { get; set; } = 1;
    }
}
