using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Model;

namespace HalceraAPI.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
