using HalceraAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace HalceraAPI.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Product>? Products { get; set; }
    }
}
