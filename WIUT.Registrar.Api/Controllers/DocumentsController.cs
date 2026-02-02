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
        [FromQuery] string? userRole) // public, student, staff, registrar
    {
        var q = _db.Documents.AsNoTracking().Where(d => d.IsActive);
        
        // Apply category filter
        if (category.HasValue) q = q.Where(d => d.Category == category.Value);
        
        // Apply visibility filter based on user role
        if (string.IsNullOrWhiteSpace(userRole) || userRole.Equals("public", StringComparison.OrdinalIgnoreCase))
        {
            // Public users see only public documents
            q = q.Where(d => d.Visibility == DocumentVisibility.Public);
        }
        else if (userRole.Equals("student", StringComparison.OrdinalIgnoreCase))
        {
            // Students see public and student-only documents
            q = q.Where(d => d.Visibility == DocumentVisibility.Public || 
                            d.Visibility == DocumentVisibility.StudentOnly);
        }
        else if (userRole.Equals("staff", StringComparison.OrdinalIgnoreCase))
        {
            // Staff see public, student, and staff-only documents
            q = q.Where(d => d.Visibility == DocumentVisibility.Public || 
                            d.Visibility == DocumentVisibility.StudentOnly ||
                            d.Visibility == DocumentVisibility.StaffOnly);
        }
        else if (userRole.Equals("registrar", StringComparison.OrdinalIgnoreCase))
        {
            // Registrar/Admin see all documents
            // No additional filter needed
        }
        else
        {
            // Unknown role - treat as public
            q = q.Where(d => d.Visibility == DocumentVisibility.Public);
        }
        
        var items = await q.OrderByDescending(d => d.CreatedAt).ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    [RequestSizeLimit(1024L * 1024L * 100L)] // 100 MB
    public async Task<ActionResult<Document>> Upload(
        [FromForm] IFormFile file, 
        [FromForm] DocumentCategory category, 
        [FromForm] int? year, 
        [FromForm] string? programmeCode,
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
            Category = category,
            Year = year,
            ProgrammeCode = programmeCode,
            Visibility = visibility ?? DocumentVisibility.Public,
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
        [FromForm] string? fileName)
    {
        var doc = await _db.Documents.FindAsync(id);
        if (doc is null) return NotFound();

        if (visibility.HasValue) doc.Visibility = visibility.Value;
        if (isActive.HasValue) doc.IsActive = isActive.Value;
        if (description != null) doc.Description = description;
        if (!string.IsNullOrWhiteSpace(fileName)) doc.FileName = fileName;
        
        doc.UpdatedAt = DateTime.UtcNow;
        
        await _db.SaveChangesAsync();
        return NoContent();
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


