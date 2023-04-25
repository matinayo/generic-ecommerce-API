namespace HalceraAPI.DataAccess.Contract
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        IApplicationUserRepository ApplicationUserRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }
        IBaseAddressRepository BaseAddressRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        Task SaveAsync();
    }
}
