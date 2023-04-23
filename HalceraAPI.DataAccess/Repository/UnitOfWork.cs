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
        }
        public IProductRepository ProductRepository { get; private set; }

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
