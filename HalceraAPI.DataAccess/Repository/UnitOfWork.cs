using HalceraAPI.DataAccess.Contract;

namespace HalceraAPI.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ProductRepository = new ProductRepository(_context);
            ApplicationUserRepository = new ApplicationUserRepository(_context);
            ShoppingCartRepository = new ShoppingCartRepository(_context);
            BaseAddressRepository = new BaseAddressRepository(_context);
            CategoryRepository = new CategoryRepository(_context);
        }
        public IProductRepository ProductRepository { get; private set; }
        public IApplicationUserRepository ApplicationUserRepository { get; private set; }

        public IShoppingCartRepository ShoppingCartRepository { get; private set; }

        public IBaseAddressRepository BaseAddressRepository { get; private set; }

        public ICategoryRepository CategoryRepository { get; private set; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void AsNoTracking()
        {
            _ = _context.ChangeTracker;
        }
    }
}
