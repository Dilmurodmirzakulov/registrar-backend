using Microsoft.AspNetCore.Mvc;
using WIUT.Registrar.Api.Services;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly IFileStorage _storage;

    public UploadController(IFileStorage storage)
    {
        _storage = storage;
    }

    [HttpPost("image")]
    [RequestSizeLimit(1024L * 1024L * 10L)] // 10 MB
    public async Task<ActionResult<object>> UploadImage([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file provided" });

        // Validate image type
        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType.ToLower()))
            return BadRequest(new { error = "Only image files are allowed" });

        var (url, size) = await _storage.SaveAsync(file, HttpContext.RequestAborted);
        var absoluteUrl = ToAbsoluteUrl(url, Request);
        
        return Ok(new { url = absoluteUrl, size, fileName = file.FileName, contentType = file.ContentType });
    }

    [HttpPost("file")]
    [RequestSizeLimit(1024L * 1024L * 50L)] // 50 MB
    public async Task<ActionResult<object>> UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file provided" });

        var allowedTypes = new[]
        {
            "application/pdf",
            "application/msword",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "image/jpeg",
            "image/jpg",
            "image/png",
            "image/gif",
            "image/webp"
        };

        if (!allowedTypes.Contains(file.ContentType.ToLower()))
            return BadRequest(new { error = "Only PDF, Word, or image files are allowed" });

        var (url, size) = await _storage.SaveAsync(file, HttpContext.RequestAborted);
        var absoluteUrl = ToAbsoluteUrl(url, Request);

        return Ok(new { url = absoluteUrl, size, fileName = file.FileName, contentType = file.ContentType });
    }

    private static string ToAbsoluteUrl(string relativePath, HttpRequest request)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return relativePath;
        if (relativePath.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return relativePath;

        var scheme = request.Scheme;
        var host = request.Host.HasValue ? request.Host.Value : string.Empty;
        return string.IsNullOrWhiteSpace(host)
            ? relativePath
            : $"{scheme}://{host}{relativePath}";
    }
}

