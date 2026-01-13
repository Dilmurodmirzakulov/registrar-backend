using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TextBlocksController : ControllerBase
{
    private readonly AppDbContext _db;

    public TextBlocksController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TextBlock>>> GetAll(
        [FromQuery] bool publishedOnly = false,
        [FromQuery] PageType? pageType = null,
        [FromQuery] string? sectionKey = null)
    {
        var query = _db.TextBlocks.AsNoTracking()
            .OrderBy(t => t.DisplayOrder)
            .ThenByDescending(t => t.CreatedAt);
        
        if (publishedOnly)
        {
            query = (IOrderedQueryable<TextBlock>)query.Where(t => t.IsPublished);
        }
        
        if (pageType.HasValue)
        {
            query = (IOrderedQueryable<TextBlock>)query.Where(t => t.PageType == pageType);
        }
        
        if (!string.IsNullOrWhiteSpace(sectionKey))
        {
            query = (IOrderedQueryable<TextBlock>)query.Where(t => t.SectionKey == sectionKey);
        }
        
        var items = await query.ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TextBlock>> GetById(int id)
    {
        var item = await _db.TextBlocks.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<TextBlock>> Create([FromBody] TextBlock dto)
    {
        dto.Id = 0;
        dto.CreatedAt = DateTime.UtcNow;
        _db.TextBlocks.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TextBlock dto)
    {
        var existing = await _db.TextBlocks.FindAsync(id);
        if (existing is null) return NotFound();
        
        existing.Title = dto.Title;
        existing.Content = dto.Content;
        existing.PageType = dto.PageType;
        existing.SectionKey = dto.SectionKey;
        existing.DisplayOrder = dto.DisplayOrder;
        existing.CssClass = dto.CssClass;
        existing.IsPublished = dto.IsPublished;
        existing.UpdatedAt = DateTime.UtcNow;
        
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _db.TextBlocks.FindAsync(id);
        if (existing is null) return NotFound();
        
        _db.TextBlocks.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}




