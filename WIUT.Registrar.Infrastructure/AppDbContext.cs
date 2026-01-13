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
    public DbSet<ImportantDate> ImportantDates => Set<ImportantDate>();
    public DbSet<PageAttachment> PageAttachments => Set<PageAttachment>();
    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();
    public DbSet<QuickLink> QuickLinks => Set<QuickLink>();
    public DbSet<Banner> Banners => Set<Banner>();
    public DbSet<FAQ> FAQs => Set<FAQ>();
    public DbSet<Statistic> Statistics => Set<Statistic>();
    public DbSet<TextBlock> TextBlocks => Set<TextBlock>();

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
        modelBuilder.Entity<ImportantDate>()
            .ToTable("ImportantDates", schema)
            .HasIndex(d => new { d.SectionKey, d.Date });

        modelBuilder.Entity<PageAttachment>()
            .ToTable("PageAttachments", schema)
            .HasOne(pa => pa.Page)
            .WithMany(p => p.Attachments)
            .HasForeignKey(pa => pa.PageId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SiteSetting>()
            .ToTable("SiteSettings", schema)
            .HasIndex(s => s.Key)
            .IsUnique();

        modelBuilder.Entity<QuickLink>()
            .ToTable("QuickLinks", schema)
            .HasIndex(q => q.DisplayOrder);

        var bannerEntity = modelBuilder.Entity<Banner>()
            .ToTable("Banners", schema);
        bannerEntity.HasIndex(b => b.DisplayOrder);
        bannerEntity.HasIndex(b => b.SectionKey);

        modelBuilder.Entity<FAQ>()
            .ToTable("FAQs", schema)
            .HasIndex(f => f.DisplayOrder);

        var statisticEntity = modelBuilder.Entity<Statistic>()
            .ToTable("Statistics", schema);
        statisticEntity.HasIndex(s => s.CardTitle);
        statisticEntity.HasIndex(s => s.DisplayOrder);

        var textBlockEntity = modelBuilder.Entity<TextBlock>()
            .ToTable("TextBlocks", schema);
        textBlockEntity.HasIndex(t => t.PageType);
        textBlockEntity.HasIndex(t => t.SectionKey);
        textBlockEntity.HasIndex(t => t.DisplayOrder);
    }
}


