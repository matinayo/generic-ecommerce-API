using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;

namespace HalceraAPI.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
