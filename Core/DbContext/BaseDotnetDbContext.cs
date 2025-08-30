namespace BaseDotnet.Core.DbContext;

using BaseDotnet.Core.Entities;
using Microsoft.EntityFrameworkCore;

public class BaseDotnetDbContext : DbContext
{
    public BaseDotnetDbContext(DbContextOptions<BaseDotnetDbContext> context) : base(context) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed default roles
        modelBuilder.Entity<Role>().HasData(
            new Role { RoleID = 1, RoleName = "Admin", Description = "System Administrator" },
            new Role { RoleID = 2, RoleName = "User", Description = "Regular User" },
            new Role { RoleID = 3, RoleName = "Guest", Description = "Guest User" }
        );
    }
}