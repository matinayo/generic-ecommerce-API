using HalceraAPI.DataAccess.Contract;
using HalceraAPI.Models;

namespace HalceraAPI.DataAccess.Repository
{
    /// <summary>
    /// Central Unit Of Work
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            ApplicationUser = new Repository<ApplicationUser>(_context);
            BaseAddress = new Repository<BaseAddress>(_context);
            Category = new Repository<Category>(_context);
            Composition = new Repository<Composition>(_context);
            CompositionData = new Repository<CompositionData>(_context);
            Media = new Repository<Media>(_context);
            Price = new Repository<Price>(_context);
            Product = new Repository<Product>(_context);
            Rating = new Repository<Rating>(_context);
            ShoppingCart = new Repository<ShoppingCart>(_context);
        }

        public IRepository<ApplicationUser> ApplicationUser { get; private set; }
        public IRepository<BaseAddress> BaseAddress { get; private set; }
        public IRepository<Category> Category { get; private set; }
        public IRepository<Composition> Composition { get; private set; }
        public IRepository<CompositionData> CompositionData { get; private set; }
        public IRepository<Media> Media { get; private set; }
        public IRepository<Price> Price { get; private set; }
        public IRepository<Product> Product { get; private set; }
        public IRepository<Rating> Rating { get; private set; }
        public IRepository<ShoppingCart> ShoppingCart { get; private set; }

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
