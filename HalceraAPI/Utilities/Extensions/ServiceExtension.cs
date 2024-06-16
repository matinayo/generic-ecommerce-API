using AutoMapper;
using HalceraAPI.Common.AppsettingsOptions;
using HalceraAPI.DataAccess;
using HalceraAPI.DataAccess.Contract;
using HalceraAPI.DataAccess.DbInitializer;
using HalceraAPI.DataAccess.Repository;
using HalceraAPI.Services.Automapper;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace HalceraAPI.Utilities.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureDbContextAsync(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                // configuration.GetSection("AppSettings:Token").Value
            });

            services.Configure<JWTOptions>(configuration.GetSection("JWTOptions"));
            services.Configure<PaystackOptions>(configuration.GetSection("PaystackOptions"));
        }

        public static void ConfigureOperationsInjection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IProductOperation, ProductOperation>();
            services.AddScoped<IShoppingCartOperation, ShoppingCartOperation>();
            services.AddScoped<ICategoryOperation, CategoryOperation>();
            services.AddScoped<IMediaOperation, MediaOperation>();
            services.AddScoped<ICompositionOperation, CompositionOperation>();
            services.AddScoped<IPriceOperation, PriceOperation>();
            services.AddScoped<IComponentDataOperation, ComponentDataOperation>();
            services.AddScoped<IIdentityOperation, IdentityOperation>();
            services.AddScoped<ICustomerOrderOperation, CustomerOrderOperation>();
            services.AddScoped<IAdminOrderOperation, AdminOrderOperation>();
            services.AddScoped<IShippingOperation, ShippingOperation>();
            services.AddScoped<IUserOperation, UserOperation>();
            services.AddScoped<IProductSizeOperation, ProductSizeOperation>();
        }

        public static void ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddEndpointsApiExplorer();
            
            services.AddAuthentication().AddJwtBearer(
                // Verify and validate issuer signing key
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            configuration.GetSection("JWTOptions:Token").Value!))
                    };
                });

            services.ConfigureSwagger();
            services.AddHttpContextAccessor();
            services.RegisterMappingProfiles();
        }

        private static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                // Setup token in Header -> Authorization
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            services.AddSwaggerGenNewtonsoftSupport();
        }

        private static void RegisterMappingProfiles(this IServiceCollection services)
        {
            List<Profile> mapperProfiles = new()
            {
                new MappingProfiles(),
                new ProductProfile(),
                new CompositionProfile(),
                new ShoppingCartProfile(),
            };

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfiles(mapperProfiles);
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
