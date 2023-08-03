using HalceraAPI.Models.Requests.Product;

namespace HalceraAPI.Models.Requests.ShoppingCart
{
    public class ShoppingCartDetailsResponse
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public ProductResponse? Product { get; set; }
    }
}
