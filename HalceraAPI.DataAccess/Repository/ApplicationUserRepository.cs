using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;

namespace HalceraAPI.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
