using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuickLinksController : ControllerBase
{
    private readonly AppDbContext _db;

    public QuickLinksController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuickLink>>> GetAll()
    {
        var links = await _db.QuickLinks.AsNoTracking()
            .OrderBy(q => q.DisplayOrder)
            .ToListAsync();
        return Ok(links);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuickLink>> GetById(int id)
    {
        var item = await _db.QuickLinks.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<QuickLink>> Create([FromBody] QuickLink dto)
    {
        dto.Id = 0;
        dto.CreatedAt = DateTime.UtcNow;
        _db.QuickLinks.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] QuickLink dto)
    {
        var existing = await _db.QuickLinks.FindAsync(id);
        if (existing is null) return NotFound();

        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.LinkUrl = dto.LinkUrl;
        existing.IconKey = dto.IconKey;
        existing.ThemeKey = dto.ThemeKey;
        existing.DisplayOrder = dto.DisplayOrder;
        existing.IsExternal = dto.IsExternal;
        existing.IsPublished = dto.IsPublished;
        existing.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _db.QuickLinks.FindAsync(id);
        if (existing is null) return NotFound();

        _db.QuickLinks.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}









