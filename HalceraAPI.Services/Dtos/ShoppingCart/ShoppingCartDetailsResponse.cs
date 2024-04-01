using HalceraAPI.Services.Dtos.Product;

namespace HalceraAPI.Services.Dtos.ShoppingCart
{
    public class ShoppingCartDetailsResponse
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public ProductResponse? Product { get; set; }
    }
}
