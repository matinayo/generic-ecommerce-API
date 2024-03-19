namespace HalceraAPI.Models.Requests.ShoppingCart
{
    public record ShoppingCartListResponse
    {
        public CartTotalResponse? CartTotal { get; set; }
        public IEnumerable<ShoppingCartDetailsResponse>? ItemsInCart { get; init; }
    }
}
