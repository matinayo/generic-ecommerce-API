using System.ComponentModel.DataAnnotations;

namespace HalceraAPI.Services.Dtos.ShoppingCart
{
    public class AddToCartRequest
    {
        public int ProductId { get; set; }

        [Range(1, 100)]
        public int? Quantity { get; set; } = 1;

        public int SelectedCompositionId { get; set; }

        public int SelectedProductSizeId { get; set; }
    }
}
