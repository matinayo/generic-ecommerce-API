namespace HalceraAPI.DataAccess.Contract
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        Task SaveAsync();
    }
}
