using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Model;

namespace HalceraAPI.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
