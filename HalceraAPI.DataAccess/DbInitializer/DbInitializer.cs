using HalceraAPI.Common.Utilities;
using HalceraAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HalceraAPI.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _dbContext;

        public DbInitializer(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            try
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    // Automatically migraate to DB
                    _dbContext.Database.Migrate();
                }
            }
            catch { }

            using var transaction = _dbContext.Database.BeginTransaction();

            if (_dbContext.Roles != null && _dbContext.Roles.Any(role => role.Title == RoleDefinition.Admin)) return;
            var applicationRoles = LoadApplicationRoles();
            if (applicationRoles != null)
            {
                var role = applicationRoles?.FirstOrDefault(role => role?.Title != null && role?.Title == RoleDefinition.Admin);
                LoadAdminUser(role);
            }
            transaction.Commit();
        }

        private List<Roles> LoadApplicationRoles()
        {
            // Create all roles required
            List<Roles> applicationRoles = new()
            {
                new() {
                    Title = RoleDefinition.Admin },
                new() {
                    Title = RoleDefinition.Employee },
                 new() {
                    Title = RoleDefinition.Customer }
            };

            _dbContext.Roles?.AddRange(applicationRoles);
            _dbContext.SaveChanges();

            return applicationRoles;
        }

        private void LoadAdminUser(Roles? roleId)
        {
            ApplicationUser applicationUser = new()
            {
                Name = "Admin",
                Email = "user@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("MASTER123*"),
                Roles = roleId != null ? new List<Roles>() { roleId } : null
            };

            _dbContext.ApplicationUsers?.Add(applicationUser);
            _dbContext.SaveChanges();
        }
    }
}
