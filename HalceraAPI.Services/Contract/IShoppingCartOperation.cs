using HalceraAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalceraAPI.Services.Contract
{
    /// <summary>
    /// Shopping Cart Operations
    /// </summary>
    public interface IShoppingCartOperation
    {
        Task<IEnumerable<ShoppingCart>?> GetAllItemsInCart();
        Task<ShoppingCart?> GetItemInCart(int shoppingCartId);
        Task<int> IncreaseItemInCart(int shoppingCartId);
        Task<int> DecreaseItemInCart(int shoppingCartId);
        Task<bool> DeleteItemInCart(int shoppingCartId);
    }
}
