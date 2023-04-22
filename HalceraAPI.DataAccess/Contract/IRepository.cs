using System.Linq.Expressions;

namespace HalceraAPI.DataAccess.Contract
{
    /// <summary>
    ///  Interface that defines base of generic Repository class
    ///   for more details, see: https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application#implement-a-generic-repository-and-a-unit-of-work-class
    /// </summary>
    public interface IRepository<T>
      where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>,
            IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null);

        Task<T?> GetFirstOrDefault(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null);

        Task Add(T entity);

        void Update(T entity);

        void Remove(T entity);
    }
}
