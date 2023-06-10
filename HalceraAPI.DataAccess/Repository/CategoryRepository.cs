using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;

namespace HalceraAPI.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
