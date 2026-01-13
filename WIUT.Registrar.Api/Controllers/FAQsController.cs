using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FAQsController : ControllerBase
{
    private readonly AppDbContext _db;

    public FAQsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FAQ>>> GetAll([FromQuery] bool publishedOnly = false, [FromQuery] string? category = null)
    {
        var query = _db.FAQs.AsNoTracking().OrderBy(f => f.DisplayOrder).ThenByDescending(f => f.CreatedAt);
        if (publishedOnly)
        {
            query = (IOrderedQueryable<FAQ>)query.Where(f => f.IsPublished);
        }
        if (!string.IsNullOrWhiteSpace(category))
        {
            query = (IOrderedQueryable<FAQ>)query.Where(f => f.Category == category);
        }
        var items = await query.ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FAQ>> GetById(int id)
    {
        var item = await _db.FAQs.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<FAQ>> Create([FromBody] FAQ dto)
    {
        dto.Id = 0;
        dto.CreatedAt = DateTime.UtcNow;
        _db.FAQs.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] FAQ dto)
    {
        var existing = await _db.FAQs.FindAsync(id);
        if (existing is null) return NotFound();
        
        existing.Question = dto.Question;
        existing.Answer = dto.Answer;
        existing.DisplayOrder = dto.DisplayOrder;
        existing.Category = dto.Category;
        existing.IsPublished = dto.IsPublished;
        existing.UpdatedAt = DateTime.UtcNow;
        
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _db.FAQs.FindAsync(id);
        if (existing is null) return NotFound();
        
        _db.FAQs.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}







