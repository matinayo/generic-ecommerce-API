using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Models.Requests.ShoppingCart
{
    public record ShoppingCartListResponse
    {
        public CartTotalResponse? CartTotal { get; set; }
        public IEnumerable<ShoppingCartDetailsResponse>? ItemsInCart { get; init; }
    }
}
