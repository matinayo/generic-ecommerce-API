using HalceraAPI.Models;

namespace HalceraAPI.DataAccess.Contract
{
    public interface IUnitOfWork
    {
        IRepository<ApplicationUser> ApplicationUser { get; }
        IRepository<BaseAddress> BaseAddress { get; }
        IRepository<Category> Category { get; }
        IRepository<Composition> Composition { get; }
        IRepository<CompositionData> CompositionData { get; }
        IRepository<Media> Media { get; }
        IRepository<Price> Price { get; }
        IRepository<Product> Product { get; }
        IRepository<Rating> Rating { get; }
        IRepository<ShoppingCart> ShoppingCart { get; }
        Task SaveAsync();
    }
}
