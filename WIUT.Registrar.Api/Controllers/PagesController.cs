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
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Page>> GetById(int id)
    {
        var item = await _db.Pages.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<Page>> GetBySlug(string slug)
    {
        var item = await _db.Pages.FirstOrDefaultAsync(p => p.Slug == slug);
        return item is null ? NotFound() : Ok(item);
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

