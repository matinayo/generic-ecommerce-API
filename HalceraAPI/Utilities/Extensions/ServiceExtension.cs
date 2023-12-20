using HalceraAPI.Common.AppsettingsOptions;
using HalceraAPI.DataAccess;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.DataAccess.DbInitializer;
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
                // configuration.GetSection("AppSettings:Token").Value
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.Configure<JWTOptions>(configuration.GetSection("JWTOptions"));
        }

        public static void ConfigureOperationsInjection(this IServiceCollection services)
        {
            services.AddScoped<IProductOperation, ProductOperation>();
            services.AddScoped<IShoppingCartOperation, ShoppingCartOperation>();
            services.AddScoped<ICategoryOperation, CategoryOperation>();
            services.AddScoped<IMediaOperation, MediaOperation>();
            services.AddScoped<ICompositionOperation, CompositionOperation>();
            services.AddScoped<IPriceOperation, PriceOperation>();
            services.AddScoped<ICompositionDataOperation, CompositionDataOperation>();
            services.AddScoped<IIdentityOperation, IdentityOperation>();
            services.AddScoped<ICustomerOrderOperation, CustomerOrderOperation>();
            services.AddScoped<IAdminOrderOperation, AdminOrderOperation>();
            services.AddScoped<IShippingOperation, ShippingOperation>();
            services.AddScoped<IUserOperation, UserOperation>();
        }
    }
}
