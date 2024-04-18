using AutoMapper;
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

        public UnitOfWork(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;

            ApplicationUser = new Repository<ApplicationUser>(_context, mapper);
            BaseAddress = new Repository<BaseAddress>(_context, mapper);
            Category = new Repository<Category>(_context, mapper);
            Composition = new Repository<Composition>(_context, mapper);
            CompositionData = new Repository<MaterialData>(_context, mapper);
            Media = new Repository<Media>(_context, mapper);
            Price = new Repository<Price>(_context, mapper);
            Product = new Repository<Product>(_context, mapper);
            Rating = new Repository<Rating>(_context, mapper);
            ShoppingCart = new Repository<ShoppingCart>(_context, mapper);
            Roles = new Repository<Roles>(_context, mapper);
            RefreshToken = new Repository<RefreshToken>(_context, mapper);
            OrderHeader = new Repository<OrderHeader>(_context, mapper);
            PaymentDetails = new Repository<PaymentDetails>(_context, mapper);
            ShippingDetails = new Repository<ShippingDetails>(_context, mapper);
            OrderDetails = new Repository<OrderDetails>(_context, mapper);
            PurchaseDetails = new Repository<PurchaseDetails>(_context, mapper);
        }

        public IRepository<ApplicationUser> ApplicationUser { get; private set; }
        public IRepository<BaseAddress> BaseAddress { get; private set; }
        public IRepository<Category> Category { get; private set; }
        public IRepository<Composition> Composition { get; private set; }
        public IRepository<MaterialData> CompositionData { get; private set; }
        public IRepository<Media> Media { get; private set; }
        public IRepository<Price> Price { get; private set; }
        public IRepository<Product> Product { get; private set; }
        public IRepository<Rating> Rating { get; private set; }
        public IRepository<ShoppingCart> ShoppingCart { get; private set; }
        public IRepository<Roles> Roles { get; private set; }
        public IRepository<RefreshToken> RefreshToken { get; private set; }
        public IRepository<OrderHeader> OrderHeader { get; private set; }
        public IRepository<PaymentDetails> PaymentDetails { get; private set; }
        public IRepository<ShippingDetails> ShippingDetails { get; private set; }
        public IRepository<OrderDetails> OrderDetails { get; private set; }
        public IRepository<PurchaseDetails> PurchaseDetails { get; private set; }

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
