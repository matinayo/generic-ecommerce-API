using AutoMapper;
using HalceraAPI.DataAccess.Contract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HalceraAPI.DataAccess.Repository
{
    /// <summary>
    /// Generic repository class
    ///  for more details, see: https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application#implement-a-generic-repository-and-a-unit-of-work-class
    /// </summary>
    /// <typeparam name="T">class</typeparam>
    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            dbSet = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            if(filter == null)
            {
                return await dbSet.CountAsync();
            }
            return await dbSet.CountAsync(filter);
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? skip = null, int? take = null, string? includeProperties = null)
        {
            IQueryable<T> query = GetAllQuery(filter, orderBy, skip, take, includeProperties);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TResult>> GetAll<TResult>(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? skip = null, int? take = null, string? includeProperties = null)
        {
            IQueryable<T> query = GetAllQuery(filter, orderBy, skip, take, includeProperties);
            return await _mapper.ProjectTo<TResult>(query).ToListAsync();
        }

        public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = GetFirstOrDefaultQuery(filter, includeProperties);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<TResult?> GetFirstOrDefault<TResult>(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = GetFirstOrDefaultQuery(filter, includeProperties);
            return await _mapper.ProjectTo<TResult>(query).FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        private IQueryable<T> GetAllQuery(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? skip = null, int? take = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (skip != null)
            {
                query = query.Skip(skip.Value);
            }
            if (take != null)
            {
                query = query.Take(take.Value);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query;
        }

        private IQueryable<T> GetFirstOrDefaultQuery(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query;
        }
    }
}
