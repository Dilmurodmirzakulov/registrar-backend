using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WIUT.Registrar.Core.Entities;

namespace WIUT.Registrar.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<News> News => Set<News>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Page> Pages => Set<Page>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<KPIReport> KPIReports => Set<KPIReport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // SQLite doesn't support schemas, so only apply schema for SQL Server
        // Check if we're using SQLite by examining the provider name
        var isSqlite = false;
        try
        {
            var providerName = Database.ProviderName ?? "";
            isSqlite = providerName.Contains("Sqlite", StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            // If provider name is not available, default to SQL Server schema
            isSqlite = false;
        }
        
        var schema = isSqlite ? null : "Registrar";
        
        if (!isSqlite)
        {
            modelBuilder.HasDefaultSchema("Registrar");
        }

        modelBuilder.Entity<News>()
            .ToTable("News", schema)
            .HasIndex(n => n.Slug)
            .IsUnique();

        modelBuilder.Entity<Page>()
            .ToTable("Pages", schema)
            .HasIndex(p => p.Slug)
            .IsUnique();

        modelBuilder.Entity<Department>()
            .ToTable("Departments", schema)
            .HasOne(d => d.ParentDepartment)
            .WithMany(d => d!.Children)
            .HasForeignKey(d => d.ParentDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TeamMember>().ToTable("TeamMembers", schema);
        modelBuilder.Entity<Document>().ToTable("DbDocuments", schema);
        modelBuilder.Entity<KPIReport>().ToTable("KPIReports", schema);
    }
}


