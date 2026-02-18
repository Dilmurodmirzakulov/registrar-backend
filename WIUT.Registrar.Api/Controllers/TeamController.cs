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

    public class TeamMemberResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Title { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? DetailsHtml { get; set; }
        public int? ManagerId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<int> ResponsiblePageIds { get; set; } = new();
    }

    public class TeamMemberUpsertDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Title { get; set; }
        public int? DepartmentId { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? DetailsHtml { get; set; }
        public int? ManagerId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
        public List<int>? ResponsiblePageIds { get; set; }
    }

    private static TeamMemberResponseDto MapTeamMember(TeamMember member)
    {
        return new TeamMemberResponseDto
        {
            Id = member.Id,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Title = member.Title,
            DepartmentId = member.DepartmentId,
            Department = member.Department,
            PhotoUrl = member.PhotoUrl,
            Email = member.Email,
            Phone = member.Phone,
            DetailsHtml = member.DetailsHtml,
            ManagerId = member.ManagerId,
            DisplayOrder = member.DisplayOrder,
            IsPublished = member.IsPublished,
            CreatedAt = member.CreatedAt,
            UpdatedAt = member.UpdatedAt,
            ResponsiblePageIds = member.ResponsiblePages
                .Select(p => p.Id)
                .OrderBy(id => id)
                .ToList()
        };
    }

    [HttpGet("members")]
    public async Task<ActionResult<IEnumerable<TeamMemberResponseDto>>> GetMembers()
    {
        var items = await _db.TeamMembers
            .AsNoTracking()
            .Include(t => t.Department)
            .Include(t => t.ResponsiblePages)
            .OrderBy(t => t.DisplayOrder)
            .ThenBy(t => t.LastName)
            .ToListAsync();
        return Ok(items.Select(MapTeamMember));
    }

    [HttpGet("members/{id:int}")]
    public async Task<ActionResult<TeamMemberResponseDto>> GetMember(int id)
    {
        var item = await _db.TeamMembers
            .AsNoTracking()
            .Include(t => t.Department)
            .Include(t => t.ResponsiblePages)
            .FirstOrDefaultAsync(t => t.Id == id);
        return item is null ? NotFound() : Ok(MapTeamMember(item));
    }

    [HttpPost("members")]
    public async Task<ActionResult<TeamMemberResponseDto>> CreateMember([FromBody] TeamMemberUpsertDto dto)
    {
        if (dto.ManagerId.HasValue)
        {
            var managerExists = await _db.TeamMembers.AnyAsync(m => m.Id == dto.ManagerId.Value);
            if (!managerExists) return BadRequest("Selected manager was not found.");
        }

        var member = new TeamMember
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Title = dto.Title,
            DepartmentId = dto.DepartmentId,
            ManagerId = dto.ManagerId,
            PhotoUrl = dto.PhotoUrl,
            Email = dto.Email,
            Phone = dto.Phone,
            DetailsHtml = dto.DetailsHtml,
            DisplayOrder = dto.DisplayOrder,
            IsPublished = dto.IsPublished,
            CreatedAt = DateTime.UtcNow
        };

        if (dto.ResponsiblePageIds is { Count: > 0 })
        {
            var pageIds = dto.ResponsiblePageIds.Distinct().ToList();
            var pages = await _db.Pages
                .Where(p => pageIds.Contains(p.Id))
                .ToListAsync();

            foreach (var page in pages)
            {
                member.ResponsiblePages.Add(page);
            }
        }

        _db.TeamMembers.Add(member);
        await _db.SaveChangesAsync();

        var created = await _db.TeamMembers
            .AsNoTracking()
            .Include(t => t.Department)
            .Include(t => t.ResponsiblePages)
            .FirstAsync(t => t.Id == member.Id);

        return CreatedAtAction(nameof(GetMember), new { id = member.Id }, MapTeamMember(created));
    }

    [HttpPut("members/{id:int}")]
    public async Task<IActionResult> UpdateMember(int id, [FromBody] TeamMemberUpsertDto dto)
    {
        var existing = await _db.TeamMembers
            .Include(t => t.ResponsiblePages)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (existing is null) return NotFound();

        if (dto.ManagerId == id)
        {
            return BadRequest("Team member cannot report to themselves.");
        }

        if (dto.ManagerId.HasValue)
        {
            var managerExists = await _db.TeamMembers.AnyAsync(m => m.Id == dto.ManagerId.Value);
            if (!managerExists) return BadRequest("Selected manager was not found.");

            var createsCycle = await CreatesManagerCycle(id, dto.ManagerId.Value);
            if (createsCycle)
            {
                return BadRequest("Invalid hierarchy: circular reporting line detected.");
            }
        }

        existing.FirstName = dto.FirstName;
        existing.LastName = dto.LastName;
        existing.Title = dto.Title;
        existing.DepartmentId = dto.DepartmentId;
        existing.ManagerId = dto.ManagerId;
        existing.PhotoUrl = dto.PhotoUrl;
        existing.Email = dto.Email;
        existing.Phone = dto.Phone;
        existing.DetailsHtml = dto.DetailsHtml;
        existing.DisplayOrder = dto.DisplayOrder;
        existing.IsPublished = dto.IsPublished;

        if (dto.ResponsiblePageIds != null)
        {
            var pageIds = dto.ResponsiblePageIds.Distinct().ToList();
            var pages = await _db.Pages
                .Where(p => pageIds.Contains(p.Id))
                .ToListAsync();

            existing.ResponsiblePages.Clear();
            foreach (var page in pages)
            {
                existing.ResponsiblePages.Add(page);
            }
        }

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

    private async Task<bool> CreatesManagerCycle(int teamMemberId, int candidateManagerId)
    {
        var current = candidateManagerId;
        while (true)
        {
            if (current == teamMemberId) return true;
            var parent = await _db.TeamMembers
                .AsNoTracking()
                .Where(m => m.Id == current)
                .Select(m => m.ManagerId)
                .FirstOrDefaultAsync();

            if (!parent.HasValue) return false;
            current = parent.Value;
        }
    }

    public class DepartmentNode
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentDepartmentId { get; set; }
        public List<DepartmentNode> Children { get; set; } = new();
    }

    public class DepartmentDto
    {
        public required string Name { get; set; }
        public int? ParentDepartmentId { get; set; }
    }

    [HttpGet("departments")]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        var items = await _db.Departments
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .ToListAsync();
        return Ok(items);
    }

    [HttpPost("departments")]
    public async Task<ActionResult<Department>> CreateDepartment([FromBody] DepartmentDto dto)
    {
        var name = dto.Name.Trim();
        if (string.IsNullOrWhiteSpace(name)) return BadRequest("Department name is required.");

        if (dto.ParentDepartmentId.HasValue)
        {
            var parentExists = await _db.Departments.AnyAsync(d => d.Id == dto.ParentDepartmentId.Value);
            if (!parentExists) return BadRequest("Selected parent department was not found.");
        }

        var entity = new Department
        {
            Name = name,
            ParentDepartmentId = dto.ParentDepartmentId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Departments.Add(entity);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDepartments), new { id = entity.Id }, entity);
    }

    [HttpPut("departments/{id:int}")]
    public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentDto dto)
    {
        var existing = await _db.Departments.FindAsync(id);
        if (existing is null) return NotFound();

        var name = dto.Name.Trim();
        if (string.IsNullOrWhiteSpace(name)) return BadRequest("Department name is required.");

        if (dto.ParentDepartmentId == id) return BadRequest("Department cannot be its own parent.");

        if (dto.ParentDepartmentId.HasValue)
        {
            var parentId = dto.ParentDepartmentId.Value;
            var parentExists = await _db.Departments.AnyAsync(d => d.Id == parentId);
            if (!parentExists) return BadRequest("Selected parent department was not found.");

            var current = parentId;
            while (true)
            {
                if (current == id) return BadRequest("Invalid hierarchy: circular reference detected.");
                var parent = await _db.Departments
                    .AsNoTracking()
                    .Where(d => d.Id == current)
                    .Select(d => d.ParentDepartmentId)
                    .FirstOrDefaultAsync();

                if (!parent.HasValue) break;
                current = parent.Value;
            }
        }

        existing.Name = name;
        existing.ParentDepartmentId = dto.ParentDepartmentId;
        existing.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("departments/{id:int}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var existing = await _db.Departments.FindAsync(id);
        if (existing is null) return NotFound();

        var hasChildren = await _db.Departments.AnyAsync(d => d.ParentDepartmentId == id);
        if (hasChildren)
        {
            return BadRequest("Cannot delete this department because it has child departments.");
        }

        var hasMembers = await _db.TeamMembers.AnyAsync(m => m.DepartmentId == id);
        if (hasMembers)
        {
            return BadRequest("Cannot delete this department because team members are assigned to it.");
        }

        _db.Departments.Remove(existing);
        await _db.SaveChangesAsync();
        return NoContent();
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
        foreach (var node in map.Values)
        {
            node.Children = node.Children.OrderBy(c => c.Name).ToList();
        }

        var roots = map.Values
            .Where(n => n.ParentDepartmentId == null)
            .OrderBy(n => n.Name)
            .ToList();
        return Ok(roots);
    }
}
