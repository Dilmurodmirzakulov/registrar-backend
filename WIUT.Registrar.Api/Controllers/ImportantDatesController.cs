using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImportantDatesController : ControllerBase
{
    private readonly AppDbContext _db;

    public ImportantDatesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ImportantDate>>> GetAll(
        [FromQuery] string? section,
        [FromQuery] bool includeAll = false)
    {
        var query = _db.ImportantDates.AsNoTracking();

        if (!includeAll)
        {
            query = query.Where(d => d.IsPublished);
        }

        if (!string.IsNullOrWhiteSpace(section))
        {
            query = query.Where(d => d.SectionKey == section);
        }

        var items = await query
            .OrderBy(d => d.Date)
            .ThenBy(d => d.SortOrder ?? int.MaxValue)
            .ThenBy(d => d.Title)
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ImportantDate>> GetById(int id)
    {
        var item = await _db.ImportantDates.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ImportantDate>> Create([FromBody] ImportantDate dto)
    {
        dto.Id = 0;
        dto.CreatedAt = DateTime.UtcNow;
        _db.ImportantDates.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ImportantDate dto)
    {
        var existing = await _db.ImportantDates.FindAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.Date = dto.Date;
        existing.SectionKey = dto.SectionKey;
        existing.LinkUrl = dto.LinkUrl;
        existing.SortOrder = dto.SortOrder;
        existing.IsPublished = dto.IsPublished;
        existing.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _db.ImportantDates.FindAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        _db.ImportantDates.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

