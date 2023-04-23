using HalceraAPI.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace HalceraAPI.Extension
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

            //  services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureOperationsInjection(this IServiceCollection services)
        {
        }
    }
}
