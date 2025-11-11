using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WIUT.Registrar.Api.Services;
using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IFileStorage _storage;

    public DocumentsController(AppDbContext db, IFileStorage storage)
    {
        _db = db;
        _storage = storage;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Document>>> Get([FromQuery] DocumentCategory? category)
    {
        var q = _db.Documents.AsNoTracking();
        if (category.HasValue) q = q.Where(d => d.Category == category.Value);
        var items = await q.OrderByDescending(d => d.CreatedAt).ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    [RequestSizeLimit(1024L * 1024L * 100L)] // 100 MB
    public async Task<ActionResult<Document>> Upload([FromForm] IFormFile file, [FromForm] DocumentCategory category, [FromForm] int? year, [FromForm] string? programmeCode)
    {
        if (file == null || file.Length == 0) return BadRequest("No file provided");
        var (url, size) = await _storage.SaveAsync(file, HttpContext.RequestAborted);
        var doc = new Document
        {
            FileName = file.FileName,
            FileUrl = url,
            FileSize = size,
            Category = category,
            Year = year,
            ProgrammeCode = programmeCode,
            CreatedAt = DateTime.UtcNow,
            IsPublished = true
        };
        _db.Documents.Add(doc);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = doc.Id }, doc);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Document>> GetById(int id)
    {
        var doc = await _db.Documents.FindAsync(id);
        return doc is null ? NotFound() : Ok(doc);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var doc = await _db.Documents.FindAsync(id);
        if (doc is null) return NotFound();
        _db.Documents.Remove(doc);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}


