using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WIUT.Registrar.Infrastructure.DesignTime;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        // Fallback connection for design-time migrations; runtime uses appsettings
        var connectionString = Environment.GetEnvironmentVariable("WIUT_REGISTRAR_CONN")
            ?? "Server=192.168.33.18;Database=INTRANET;User Id=srstest;Password=0nceyougetcaughtyoudie123;MultipleActiveResultSets=True;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectionString);
        return new AppDbContext(optionsBuilder.Options);
    }
}


