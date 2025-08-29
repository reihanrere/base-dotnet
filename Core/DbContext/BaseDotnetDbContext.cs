namespace BaseDotnet.Core.DbContext;

using BaseDotnet.Core.Entities;
using Microsoft.EntityFrameworkCore;

public class BaseDotnetDbContext : DbContext
{
    public BaseDotnetDbContext(DbContextOptions<BaseDotnetDbContext> context) : base(context) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RolePageAccess> RolePageAccesses { get; set; }
    public DbSet<RolePage> RolePages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RolePageAccess>().HasKey(x => new { x.PageCode, x.Action, x.RoleID });
        modelBuilder.Entity<RolePageAccess>().ToTable("mst_role_page_access", "master");
    }
}