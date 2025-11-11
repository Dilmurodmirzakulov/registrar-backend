using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    private readonly AppDbContext _db;

    public NewsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<News>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;
        var query = _db.News.AsNoTracking().OrderByDescending(n => n.PublishedAt ?? n.CreatedAt);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return Ok(items);
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<News>> GetBySlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return BadRequest();
        var s = slug.Trim().ToLower();
        var item = await _db.News
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Slug.ToLower() == s);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<News>> GetById(int id)
    {
        var item = await _db.News.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<News>> Create([FromBody] News dto)
    {
        dto.Id = 0;
        dto.CreatedAt = DateTime.UtcNow;
        _db.News.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] News dto)
    {
        var existing = await _db.News.FindAsync(id);
        if (existing is null) return NotFound();
        existing.Title = dto.Title;
        existing.Slug = dto.Slug;
        existing.Summary = dto.Summary;
        existing.BodyHtml = dto.BodyHtml;
        existing.CoverImageUrl = dto.CoverImageUrl;
        existing.PublishedAt = dto.PublishedAt;
        existing.TagsCsv = dto.TagsCsv;
        existing.IsPublished = dto.IsPublished;
        existing.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existing = await _db.News.FindAsync(id);
        if (existing is null) return NotFound();
        _db.News.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
