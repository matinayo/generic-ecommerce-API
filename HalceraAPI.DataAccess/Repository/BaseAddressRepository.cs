using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Model;

namespace HalceraAPI.DataAccess.Repository
{
    public class BaseAddressRepository : Repository<BaseAddress>, IBaseAddressRepository
    {
        public BaseAddressRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
