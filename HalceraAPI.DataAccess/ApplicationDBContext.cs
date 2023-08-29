using HalceraAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HalceraAPI.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<BaseAddress>? BaseAddresses { get; set; }
        public DbSet<ShoppingCart>? ShoppingCart { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Composition>? Compositions { get; set; }
        public DbSet<CompositionData>? CompositionData { get; set; }
        public DbSet<Media>? Medias { get; set; }
        public DbSet<Price>? Prices { get; set; }
        public DbSet<Rating>? Ratings { get; set; }
        public DbSet<Roles>? Roles { get; set; }
        public DbSet<RefreshToken>? RefreshTokens { get; set; }
        public DbSet<OrderHeader>? OrderHeaders { get; set; }
        public DbSet<OrderDetails>? OrderDetails { get; set; }
        public DbSet<PurchaseDetails>? PurchaseDetails { get; set; }
        public DbSet<PaymentDetails>? PaymentDetails { get; set; }
        public DbSet<ShippingDetails>? ShippingDetails { get; set; }

    }
}
