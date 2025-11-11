using Microsoft.EntityFrameworkCore;
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
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (builder.Environment.IsDevelopment() && connectionString?.Contains("Data Source") == true)
    {
        // Use SQLite for local development
        options.UseSqlite(connectionString);
    }
    else
    {
        // Use SQL Server for production
        options.UseSqlServer(connectionString);
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
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var isSqlite = builder.Environment.IsDevelopment() && connectionString?.Contains("Data Source") == true;
    
    if (isSqlite)
    {
        // For SQLite in development, use EnsureCreated (no migrations needed)
        db.Database.EnsureCreated();
    }
    else
    {
        // For SQL Server, use migrations
        db.Database.Migrate();
    }
    
    DbSeeder.Seed(db);
}

app.Run();
