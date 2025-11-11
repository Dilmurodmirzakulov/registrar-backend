using Microsoft.AspNetCore.Http;

namespace WIUT.Registrar.Api.Services;

public interface IFileStorage
{
    Task<(string fileUrl, long fileSize)> SaveAsync(IFormFile file, CancellationToken ct = default);
}

public class LocalFileStorage : IFileStorage
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<LocalFileStorage> _logger;

    public LocalFileStorage(IWebHostEnvironment env, ILogger<LocalFileStorage> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async Task<(string fileUrl, long fileSize)> SaveAsync(IFormFile file, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        var uploadsRoot = Path.Combine(_env.ContentRootPath, "uploads", now.Year.ToString(), now.Month.ToString("D2"));
        Directory.CreateDirectory(uploadsRoot);

        var safeName = string.Concat(Path.GetFileNameWithoutExtension(file.FileName)
            .Select(ch => char.IsLetterOrDigit(ch) ? ch : '-'));
        safeName = string.Join('-', safeName.Split('-', StringSplitOptions.RemoveEmptyEntries));
        var unique = $"{safeName}-{Guid.NewGuid().ToString("N").Substring(0,8)}{Path.GetExtension(file.FileName)}";
        var savePath = Path.Combine(uploadsRoot, unique);

        await using (var stream = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await file.CopyToAsync(stream, ct);
        }

        var relative = $"/uploads/{now.Year}/{now.Month:D2}/{unique}";
        var info = new FileInfo(savePath);
        _logger.LogInformation("Saved upload {File} ({Size} bytes)", relative, info.Length);
        return (relative, info.Length);
    }
}


