using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BannersController : ControllerBase
{
    private readonly AppDbContext _db;

    public BannersController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Banner>>> GetAll([FromQuery] bool publishedOnly = false, [FromQuery] string? sectionKey = null)
    {
        var query = _db.Banners.AsNoTracking().OrderBy(b => b.DisplayOrder).ThenByDescending(b => b.CreatedAt);
        if (publishedOnly)
        {
            query = (IOrderedQueryable<Banner>)query.Where(b => b.IsPublished);
        }
        if (!string.IsNullOrWhiteSpace(sectionKey))
        {
            query = (IOrderedQueryable<Banner>)query.Where(b => b.SectionKey == sectionKey);
        }
        var items = await query.ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Banner>> GetById(int id)
    {
        var item = await _db.Banners.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Banner>> Create([FromBody] Banner dto)
    {
        dto.Id = 0;
        dto.CreatedAt = DateTime.UtcNow;
        _db.Banners.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Banner dto)
    {
        var existing = await _db.Banners.FindAsync(id);
        if (existing is null) return NotFound();
        
        existing.ImageUrl = dto.ImageUrl;
        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.DisplayOrder = dto.DisplayOrder;
        existing.LinkUrl = dto.LinkUrl;
        existing.SectionKey = dto.SectionKey;
        existing.IsPublished = dto.IsPublished;
        existing.UpdatedAt = DateTime.UtcNow;
        
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _db.Banners.FindAsync(id);
        if (existing is null) return NotFound();
        
        _db.Banners.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

