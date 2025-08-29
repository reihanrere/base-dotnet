namespace BaseDotnet.DbContext;

using Microsoft.EntityFrameworkCore;
using BaseDotnet.Entities;

public class BaseDotnetDbContext : DbContext
{
    public BaseDotnetDbContext(DbContextOptions<BaseDotnetDbContext> context) : base(context) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePageAccess> RolePageAccesses { get; set; }
    public DbSet<RolePage> RolePages { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<Office> Office { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RolePageAccess>().HasKey(x => new { x.PageCode, x.Action, x.RoleID });
        modelBuilder.Entity<RolePageAccess>().ToTable("mst_role_page_access", "master");
    }
}