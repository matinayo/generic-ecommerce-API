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
        Task<IList<T>> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>,
            IOrderedQueryable<T>>? orderBy = null, int? skip = null, int? take = null, string? includeProperties = null);

        /// <summary>
        /// Select with defined Entity of different types: T and TResult
        /// </summary>
        Task<IList<TResult>> GetAll<TResult>(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? skip = null, int? take = null, string? includeProperties = null);

        Task<T?> GetFirstOrDefault(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null);

        /// <summary>
        /// Select with defined Entity 
        /// </summary>
        Task<TResult?> GetFirstOrDefault<TResult>(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null);

        Task Add(T entity);

        Task AddRange(IList<T> entities);

        void Update(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);
    }
}
