using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SiteSettingsController : ControllerBase
{
    private readonly AppDbContext _db;

    public SiteSettingsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SiteSetting>>> GetAll()
    {
        var settings = await _db.SiteSettings.AsNoTracking()
            .OrderBy(s => s.Key)
            .ToListAsync();
        return Ok(settings);
    }

    [HttpGet("{key}")]
    public async Task<ActionResult<SiteSetting>> GetByKey(string key)
    {
        var item = await _db.SiteSettings.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Key == key);
        return item is null ? NotFound() : Ok(item);
    }

    public record SiteSettingDto(string Value, string? Category, string? Description);

    [HttpPut("{key}")]
    public async Task<ActionResult<SiteSetting>> Upsert(string key, [FromBody] SiteSettingDto dto)
    {
        var existing = await _db.SiteSettings.FirstOrDefaultAsync(s => s.Key == key);
        if (existing is null)
        {
            var setting = new SiteSetting
            {
                Key = key,
                Value = dto.Value,
                Category = dto.Category,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };
            _db.SiteSettings.Add(setting);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetByKey), new { key }, setting);
        }

        existing.Value = dto.Value;
        existing.Category = dto.Category;
        existing.Description = dto.Description;
        existing.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return Ok(existing);
    }
}









