using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.FileProviders;
using Serilog;
using WIUT.Registrar.Infrastructure;
using WIUT.Registrar.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.Console());

// Services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var useSqlite = builder.Environment.IsDevelopment() && defaultConnection?.Contains("Data Source", StringComparison.OrdinalIgnoreCase) == true;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (useSqlite)
    {
        // Use SQLite for local development
        options.UseSqlite(defaultConnection);
    }
    else
    {
        // Use SQL Server for production
        options.UseSqlServer(defaultConnection);
    }
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", policy =>
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://127.0.0.1:5173",
                "https://registrar-front-wk3o.vercel.app")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddSingleton<IFileStorage, LocalFileStorage>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
// In development, keep HTTP enabled for local testing
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Serve static files from wwwroot
app.UseStaticFiles();

// Serve static files from uploads directory
var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "uploads");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

app.UseCors("Default");

app.MapGet("/api/health", () => Results.Ok(new { status = "ok", time = DateTime.UtcNow }))
   .WithName("Health");

app.MapControllers();

// Create schema/tables for our namespace without touching existing ones
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (useSqlite)
    {
        db.Database.EnsureCreated();
        EnsureSqliteTable(db, "ImportantDates",
            """
            CREATE TABLE IF NOT EXISTS ImportantDates (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT NULL,
                Date TEXT NOT NULL,
                SectionKey TEXT NOT NULL,
                LinkUrl TEXT NULL,
                SortOrder INTEGER NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NULL,
                IsPublished INTEGER NOT NULL
            );
            """,
            """
            CREATE INDEX IF NOT EXISTS IX_ImportantDates_SectionKey_Date
            ON ImportantDates (SectionKey, Date);
            """);

        EnsureSqliteTable(db, "PageAttachments",
            """
            CREATE TABLE IF NOT EXISTS PageAttachments (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PageId INTEGER NOT NULL,
                Title TEXT NOT NULL,
                Caption TEXT NULL,
                FileUrl TEXT NOT NULL,
                FileName TEXT NOT NULL,
                FileSize INTEGER NOT NULL,
                ContentType TEXT NOT NULL,
                IsImage INTEGER NOT NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NULL,
                IsPublished INTEGER NOT NULL,
                FOREIGN KEY (PageId) REFERENCES Pages(Id) ON DELETE CASCADE
            );
            """,
            """
            CREATE INDEX IF NOT EXISTS IX_PageAttachments_PageId
            ON PageAttachments (PageId);
            """);

        EnsureSqliteTable(db, "SiteSettings",
            """
            CREATE TABLE IF NOT EXISTS SiteSettings (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Key TEXT NOT NULL,
                Value TEXT NULL,
                Category TEXT NULL,
                Description TEXT NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NULL,
                IsPublished INTEGER NOT NULL
            );
            """,
            """
            CREATE UNIQUE INDEX IF NOT EXISTS IX_SiteSettings_Key
            ON SiteSettings (Key);
            """);

        EnsureSqliteTable(db, "QuickLinks",
            """
            CREATE TABLE IF NOT EXISTS QuickLinks (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT NULL,
                LinkUrl TEXT NOT NULL,
                IconKey TEXT NOT NULL,
                ThemeKey TEXT NOT NULL,
                DisplayOrder INTEGER NOT NULL,
                IsExternal INTEGER NOT NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NULL,
                IsPublished INTEGER NOT NULL
            );
            """,
            """
            CREATE INDEX IF NOT EXISTS IX_QuickLinks_DisplayOrder
            ON QuickLinks (DisplayOrder);
            """);

        EnsureSqliteTable(db, "Banners",
            """
            CREATE TABLE IF NOT EXISTS Banners (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ImageUrl TEXT NOT NULL,
                Title TEXT NOT NULL,
                Description TEXT NULL,
                DisplayOrder INTEGER NOT NULL,
                LinkUrl TEXT NULL,
                SectionKey TEXT NOT NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NULL,
                IsPublished INTEGER NOT NULL
            );
            """,
            """
            CREATE INDEX IF NOT EXISTS IX_Banners_DisplayOrder
            ON Banners (DisplayOrder);
            CREATE INDEX IF NOT EXISTS IX_Banners_SectionKey
            ON Banners (SectionKey);
            """);

        // Migrate existing Banners table to add SectionKey if missing
        EnsureSqliteColumn(db, "Banners", "SectionKey", "TEXT NOT NULL DEFAULT 'general'");

        EnsureSqliteTable(db, "FAQs",
            """
            CREATE TABLE IF NOT EXISTS FAQs (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Question TEXT NOT NULL,
                Answer TEXT NOT NULL,
                DisplayOrder INTEGER NOT NULL,
                Category TEXT NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NULL,
                IsPublished INTEGER NOT NULL
            );
            """,
            """
            CREATE INDEX IF NOT EXISTS IX_FAQs_DisplayOrder
            ON FAQs (DisplayOrder);
            """);

        EnsureSqliteTable(db, "Statistics",
            """
            CREATE TABLE IF NOT EXISTS Statistics (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                CardTitle TEXT NOT NULL,
                StatName TEXT NOT NULL,
                StatValue TEXT NOT NULL,
                Description TEXT NULL,
                DisplayOrder INTEGER NOT NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NULL,
                IsPublished INTEGER NOT NULL
            );
            """,
            """
            CREATE INDEX IF NOT EXISTS IX_Statistics_CardTitle
            ON Statistics (CardTitle);
            CREATE INDEX IF NOT EXISTS IX_Statistics_DisplayOrder
            ON Statistics (DisplayOrder);
            """);

        EnsureSqliteTable(db, "TextBlocks",
            """
            CREATE TABLE IF NOT EXISTS TextBlocks (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Content TEXT NOT NULL,
                PageType INTEGER NULL,
                SectionKey TEXT NULL,
                DisplayOrder INTEGER NOT NULL,
                CssClass TEXT NULL,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NULL,
                IsPublished INTEGER NOT NULL
            );
            """,
            """
            CREATE INDEX IF NOT EXISTS IX_TextBlocks_PageType
            ON TextBlocks (PageType);
            CREATE INDEX IF NOT EXISTS IX_TextBlocks_SectionKey
            ON TextBlocks (SectionKey);
            CREATE INDEX IF NOT EXISTS IX_TextBlocks_DisplayOrder
            ON TextBlocks (DisplayOrder);
            """);
    }
    else
    {
        db.Database.Migrate();
    }

    DbSeeder.Seed(db);
}

static void EnsureSqliteTable(AppDbContext db, string tableName, string createSql, string? indexSql = null)
{
    var connection = db.Database.GetDbConnection();
    connection.Open();
    using var checkCmd = connection.CreateCommand();
    checkCmd.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";
    var exists = checkCmd.ExecuteScalar();
    if (exists != null)
    {
        connection.Close();
        return;
    }

    db.Database.ExecuteSqlRaw(createSql);

    if (!string.IsNullOrWhiteSpace(indexSql))
    {
        db.Database.ExecuteSqlRaw(indexSql);
    }
    connection.Close();
}

static void EnsureSqliteColumn(AppDbContext db, string tableName, string columnName, string columnDefinition)
{
    var connection = db.Database.GetDbConnection();
    connection.Open();
    try
    {
        // Check if column exists
        using var checkCmd = connection.CreateCommand();
        checkCmd.CommandText = $"PRAGMA table_info({tableName});";
        using var reader = checkCmd.ExecuteReader();
        bool columnExists = false;
        while (reader.Read())
        {
            if (reader.GetString(1) == columnName) // Column name is at index 1
            {
                columnExists = true;
                break;
            }
        }
        reader.Close();

        // Add column if it doesn't exist
        if (!columnExists)
        {
            // Note: tableName, columnName, and columnDefinition are hardcoded values, not user input
            // Using ExecuteSqlInterpolated with FormattableString to satisfy the analyzer
            db.Database.ExecuteSqlInterpolated($"ALTER TABLE {tableName} ADD COLUMN {columnName} {columnDefinition};");
        }
    }
    finally
    {
        connection.Close();
    }
}

app.Run();
