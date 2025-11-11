using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly AppDbContext _db;

    public TeamController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("members")]
    public async Task<ActionResult<IEnumerable<TeamMember>>> GetMembers()
    {
        var items = await _db.TeamMembers
            .AsNoTracking()
            .Include(t => t.Department)
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.LastName)
            .ToListAsync();
        return Ok(items);
    }

    [HttpGet("members/{id:int}")]
    public async Task<ActionResult<TeamMember>> GetMember(int id)
    {
        var item = await _db.TeamMembers.FindAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost("members")]
    public async Task<ActionResult<TeamMember>> CreateMember([FromBody] TeamMember dto)
    {
        dto.Id = 0;
        dto.CreatedAt = DateTime.UtcNow;
        _db.TeamMembers.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMember), new { id = dto.Id }, dto);
    }

    [HttpPut("members/{id:int}")]
    public async Task<IActionResult> UpdateMember(int id, [FromBody] TeamMember dto)
    {
        var existing = await _db.TeamMembers.FindAsync(id);
        if (existing is null) return NotFound();
        existing.FirstName = dto.FirstName;
        existing.LastName = dto.LastName;
        existing.Title = dto.Title;
        existing.DepartmentId = dto.DepartmentId;
        existing.PhotoUrl = dto.PhotoUrl;
        existing.Email = dto.Email;
        existing.Phone = dto.Phone;
        existing.DisplayOrder = dto.DisplayOrder;
        existing.IsPublished = dto.IsPublished;
        existing.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("members/{id:int}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        var existing = await _db.TeamMembers.FindAsync(id);
        if (existing is null) return NotFound();
        _db.TeamMembers.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    public class DepartmentNode
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentDepartmentId { get; set; }
        public List<DepartmentNode> Children { get; set; } = new();
    }

    [HttpGet("departments/tree")]
    public async Task<ActionResult<IEnumerable<DepartmentNode>>> GetDepartmentTree()
    {
        var depts = await _db.Departments.AsNoTracking().ToListAsync();
        var map = depts.ToDictionary(d => d.Id, d => new DepartmentNode
        {
            Id = d.Id,
            Name = d.Name,
            ParentDepartmentId = d.ParentDepartmentId
        });
        foreach (var d in map.Values)
        {
            if (d.ParentDepartmentId.HasValue && map.TryGetValue(d.ParentDepartmentId.Value, out var parent))
            {
                parent.Children.Add(d);
            }
        }
        var roots = map.Values.Where(n => n.ParentDepartmentId == null).ToList();
        return Ok(roots);
    }
}
