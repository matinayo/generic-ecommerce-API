using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Dtos.ShoppingCart
{
    public record ShoppingCartUpdateResponse
    {
        public int Quantity { get; init; }
        public CartTotalResponse? CartTotal { get; init; }
    }
}
