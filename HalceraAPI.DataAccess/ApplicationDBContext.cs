using HalceraAPI.Model;
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
        public DbSet<Category>? Category { get; set; }
    }
}
