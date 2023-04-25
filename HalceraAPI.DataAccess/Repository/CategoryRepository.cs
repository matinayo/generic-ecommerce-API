using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Model;

namespace HalceraAPI.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
