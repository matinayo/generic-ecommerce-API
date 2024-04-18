using HalceraAPI.Models;

namespace HalceraAPI.DataAccess.Contract
{
    public interface IUnitOfWork
    {
        IRepository<ApplicationUser> ApplicationUser { get; }
        IRepository<BaseAddress> BaseAddress { get; }
        IRepository<Category> Category { get; }
        IRepository<Composition> Composition { get; }
        IRepository<MaterialData> CompositionData { get; }
        IRepository<Media> Media { get; }
        IRepository<Price> Price { get; }
        IRepository<Product> Product { get; }
        IRepository<Rating> Rating { get; }
        IRepository<ShoppingCart> ShoppingCart { get; }
        IRepository<Roles> Roles { get; }
        IRepository<RefreshToken> RefreshToken { get; }
        public IRepository<OrderHeader> OrderHeader { get; }
        public IRepository<PaymentDetails> PaymentDetails { get; }
        public IRepository<ShippingDetails> ShippingDetails { get; }
        public IRepository<OrderDetails> OrderDetails { get; }
        public IRepository<PurchaseDetails> PurchaseDetails { get; }
        Task SaveAsync();
    }
}
