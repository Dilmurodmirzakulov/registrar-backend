using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly AppDbContext _db;

    public StatisticsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Statistic>>> GetAll([FromQuery] bool publishedOnly = false)
    {
        var query = _db.Statistics.AsNoTracking()
            .OrderBy(s => s.CardTitle)
            .ThenBy(s => s.DisplayOrder)
            .ThenByDescending(s => s.CreatedAt);
        if (publishedOnly)
        {
            query = (IOrderedQueryable<Statistic>)query.Where(s => s.IsPublished);
        }
        var items = await query.ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Statistic>> GetById(int id)
    {
        var item = await _db.Statistics.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Statistic>> Create([FromBody] Statistic dto)
    {
        dto.Id = 0;
        dto.CreatedAt = DateTime.UtcNow;
        _db.Statistics.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Statistic dto)
    {
        var existing = await _db.Statistics.FindAsync(id);
        if (existing is null) return NotFound();
        
        existing.CardTitle = dto.CardTitle;
        existing.StatName = dto.StatName;
        existing.StatValue = dto.StatValue;
        existing.Description = dto.Description;
        existing.DisplayOrder = dto.DisplayOrder;
        existing.IsPublished = dto.IsPublished;
        existing.UpdatedAt = DateTime.UtcNow;
        
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _db.Statistics.FindAsync(id);
        if (existing is null) return NotFound();
        
        _db.Statistics.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}







