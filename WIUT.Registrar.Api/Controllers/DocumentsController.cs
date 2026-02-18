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
    public async Task<ActionResult<IEnumerable<Document>>> Get(
        [FromQuery] DocumentCategory? category,
        [FromQuery] string? userRole) // student, staff, registrar
    {
        var q = _db.Documents.AsNoTracking();
        
        // Apply category filter
        if (category.HasValue) q = q.Where(d => d.Category == category.Value);
        
        // Apply visibility filter based on user role
        if (string.IsNullOrWhiteSpace(userRole) || userRole.Equals("public", StringComparison.OrdinalIgnoreCase))
        {
            // Public users do not have access to protected documents
            q = q.Where(_ => false);
        }
        else if (userRole.Equals("student", StringComparison.OrdinalIgnoreCase))
        {
            // Students see student-only documents (and legacy public records mapped as protected)
            q = q.Where(d => d.IsActive && (d.Visibility == DocumentVisibility.StudentOnly || (int)d.Visibility == 0));
        }
        else if (userRole.Equals("staff", StringComparison.OrdinalIgnoreCase))
        {
            // Staff see student and staff-only documents (and legacy public records mapped as protected)
            q = q.Where(d => d.IsActive && (
                            ((int)d.Visibility == 0) ||
                            d.Visibility == DocumentVisibility.StudentOnly ||
                            d.Visibility == DocumentVisibility.StaffOnly));
        }
        else if (userRole.Equals("registrar", StringComparison.OrdinalIgnoreCase))
        {
            // Registrar/Admin see all documents including inactive (archived)
            // No additional filter needed
        }
        else
        {
            // Unknown role - no access
            q = q.Where(_ => false);
        }
        
        var items = await q
            .OrderBy(d => d.Position)
            .ThenByDescending(d => d.CreatedAt)
            .ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    [RequestSizeLimit(1024L * 1024L * 100L)] // 100 MB
    public async Task<ActionResult<Document>> Upload(
        [FromForm] IFormFile file, 
        [FromForm] DocumentCategory category, 
        [FromForm] int? year, 
        [FromForm] string? programmeCode,
        [FromForm] int? position,
        [FromForm] DocumentVisibility? visibility,
        [FromForm] bool? isActive,
        [FromForm] string? description)
    {
        if (file == null || file.Length == 0) return BadRequest("No file provided");
        var (url, size) = await _storage.SaveAsync(file, HttpContext.RequestAborted);
        var doc = new Document
        {
            FileName = file.FileName,
            FileUrl = url,
            FileSize = size,
            Position = position ?? 0,
            Category = category,
            Year = year,
            ProgrammeCode = programmeCode,
            Visibility = NormalizeVisibility(visibility),
            IsActive = isActive ?? true,
            Description = description,
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

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromForm] DocumentVisibility? visibility,
        [FromForm] bool? isActive,
        [FromForm] string? description,
        [FromForm] string? fileName,
        [FromForm] int? position)
    {
        var doc = await _db.Documents.FindAsync(id);
        if (doc is null) return NotFound();

        if (visibility.HasValue) doc.Visibility = NormalizeVisibility(visibility);
        if (isActive.HasValue) doc.IsActive = isActive.Value;
        if (description != null) doc.Description = description;
        if (!string.IsNullOrWhiteSpace(fileName)) doc.FileName = fileName;
        if (position.HasValue) doc.Position = position.Value;
        
        doc.UpdatedAt = DateTime.UtcNow;
        
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static DocumentVisibility NormalizeVisibility(DocumentVisibility? visibility)
    {
        var raw = (int)(visibility ?? DocumentVisibility.StudentOnly);
        return raw switch
        {
            2 => DocumentVisibility.StaffOnly,
            3 => DocumentVisibility.Restricted,
            _ => DocumentVisibility.StudentOnly
        };
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


