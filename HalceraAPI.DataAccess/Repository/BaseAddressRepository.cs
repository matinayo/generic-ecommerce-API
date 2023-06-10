using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;

namespace HalceraAPI.DataAccess.Repository
{
    public class BaseAddressRepository : Repository<BaseAddress>, IBaseAddressRepository
    {
        public BaseAddressRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
