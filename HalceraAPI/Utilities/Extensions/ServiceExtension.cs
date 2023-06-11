using HalceraAPI.DataAccess;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.DataAccess.Repository;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Operations;
using Microsoft.EntityFrameworkCore;

namespace HalceraAPI.Utilities.Extensions
{
    /// <summary>
    /// Builder Services for dependencies
    /// </summary>
    public static class ServiceExtension
    {
        public static void ConfigureDbContextAsync(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureOperationsInjection(this IServiceCollection services)
        {
            services.AddScoped<IProductOperation, ProductOperation>();
            services.AddScoped<IShoppingCartOperation, ShoppingCartOperation>();
            services.AddScoped<ICategoryOperation, CategoryOperation>();
        }
    }
}
