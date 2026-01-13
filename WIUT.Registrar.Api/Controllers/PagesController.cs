using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagesController : ControllerBase
{
    private readonly AppDbContext _db;

    public PagesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Page>>> GetAll([FromQuery] PageType? type)
    {
        var query = _db.Pages.AsNoTracking().OrderBy(p => p.Title);
        if (type.HasValue)
            query = (IOrderedQueryable<Page>)query.Where(p => p.Type == type.Value);
        
        var items = await query.ToListAsync();
        var pageIds = items.Select(p => p.Id).ToList();
        
        // Load attachments separately to avoid loading Page navigation property
        var allAttachments = await _db.PageAttachments
            .AsNoTracking()
            .Where(a => pageIds.Contains(a.PageId))
            .ToListAsync();
        
        // Assign attachments to pages without Page navigation
        foreach (var page in items)
        {
            page.Attachments = allAttachments
                .Where(a => a.PageId == page.Id)
                .OrderBy(a => a.IsImage ? 0 : 1)
                .ThenByDescending(a => a.CreatedAt)
                .Select(a => new PageAttachment
                {
                    Id = a.Id,
                    PageId = a.PageId,
                    Title = a.Title,
                    Caption = a.Caption,
                    FileUrl = a.FileUrl,
                    FileName = a.FileName,
                    FileSize = a.FileSize,
                    ContentType = a.ContentType,
                    IsImage = a.IsImage,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt,
                    IsPublished = a.IsPublished,
                    Page = null // Explicitly set to null
                }).ToList();
        }
        
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Page>> GetById(int id)
    {
        var item = await _db.Pages
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
        if (item == null) return NotFound();
        
        // Load attachments separately to avoid loading Page navigation property
        var attachments = await _db.PageAttachments
            .AsNoTracking()
            .Where(a => a.PageId == id)
            .OrderBy(a => a.IsImage ? 0 : 1)
            .ThenByDescending(a => a.CreatedAt)
            .ToListAsync();
        
        // Create new attachment list without Page navigation
        item.Attachments = attachments.Select(a => new PageAttachment
        {
            Id = a.Id,
            PageId = a.PageId,
            Title = a.Title,
            Caption = a.Caption,
            FileUrl = a.FileUrl,
            FileName = a.FileName,
            FileSize = a.FileSize,
            ContentType = a.ContentType,
            IsImage = a.IsImage,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
            IsPublished = a.IsPublished,
            Page = null // Explicitly set to null
        }).ToList();
        
        return Ok(item);
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<Page>> GetBySlug(string slug)
    {
        var item = await _db.Pages
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Slug == slug);
        if (item == null) return NotFound();
        
        // Load attachments separately to avoid loading Page navigation property
        var attachments = await _db.PageAttachments
            .AsNoTracking()
            .Where(a => a.PageId == item.Id)
            .OrderBy(a => a.IsImage ? 0 : 1)
            .ThenByDescending(a => a.CreatedAt)
            .ToListAsync();
        
        // Create new attachment list without Page navigation
        item.Attachments = attachments.Select(a => new PageAttachment
        {
            Id = a.Id,
            PageId = a.PageId,
            Title = a.Title,
            Caption = a.Caption,
            FileUrl = a.FileUrl,
            FileName = a.FileName,
            FileSize = a.FileSize,
            ContentType = a.ContentType,
            IsImage = a.IsImage,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt,
            IsPublished = a.IsPublished,
            Page = null // Explicitly set to null
        }).ToList();
        
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Page>> Create([FromBody] Page dto)
    {
        dto.Id = 0;
        dto.CreatedAt = DateTime.UtcNow;
        _db.Pages.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Page dto)
    {
        var existing = await _db.Pages.FindAsync(id);
        if (existing is null) return NotFound();
        
        existing.Title = dto.Title;
        existing.Slug = dto.Slug;
        existing.BodyHtml = dto.BodyHtml;
        existing.Type = dto.Type;
        existing.UpdatedAt = DateTime.UtcNow;
        
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _db.Pages.FindAsync(id);
        if (existing is null) return NotFound();
        
        _db.Pages.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }

}

