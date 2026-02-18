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

    public class TeamMemberSummaryDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Title { get; set; }
        public string? DepartmentName { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class PageResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public PageType Type { get; set; }
        public string? BodyHtml { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsPublished { get; set; }
        public List<PageAttachment> Attachments { get; set; } = new();
        public List<TeamMemberSummaryDto> ResponsibleTeamMembers { get; set; } = new();
    }

    public class PageUpsertDto
    {
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public PageType Type { get; set; }
        public string? BodyHtml { get; set; }
        public List<int>? ResponsibleTeamMemberIds { get; set; }
    }

    private static PageAttachment MapAttachment(PageAttachment attachment)
    {
        return new PageAttachment
        {
            Id = attachment.Id,
            PageId = attachment.PageId,
            Position = attachment.Position,
            Title = attachment.Title,
            Caption = attachment.Caption,
            FileUrl = attachment.FileUrl,
            FileName = attachment.FileName,
            FileSize = attachment.FileSize,
            ContentType = attachment.ContentType,
            IsImage = attachment.IsImage,
            CreatedAt = attachment.CreatedAt,
            UpdatedAt = attachment.UpdatedAt,
            IsPublished = attachment.IsPublished,
            Page = null
        };
    }

    private static TeamMemberSummaryDto MapTeamMember(TeamMember member)
    {
        return new TeamMemberSummaryDto
        {
            Id = member.Id,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Title = member.Title,
            DepartmentName = member.Department?.Name,
            PhotoUrl = member.PhotoUrl,
            Email = member.Email,
            Phone = member.Phone,
            DisplayOrder = member.DisplayOrder
        };
    }

    private static PageResponseDto MapPage(Page page)
    {
        return new PageResponseDto
        {
            Id = page.Id,
            Title = page.Title,
            Slug = page.Slug,
            Type = page.Type,
            BodyHtml = page.BodyHtml,
            CreatedAt = page.CreatedAt,
            UpdatedAt = page.UpdatedAt,
            IsPublished = page.IsPublished,
            Attachments = page.Attachments
                .OrderBy(a => a.Position)
                .ThenBy(a => a.IsImage ? 0 : 1)
                .ThenByDescending(a => a.CreatedAt)
                .Select(MapAttachment)
                .ToList(),
            ResponsibleTeamMembers = page.ResponsibleTeamMembers
                .OrderBy(m => m.DisplayOrder)
                .ThenBy(m => m.LastName)
                .ThenBy(m => m.FirstName)
                .Select(MapTeamMember)
                .ToList()
        };
    }

    private static string NormalizeSlug(string slug)
    {
        return slug.Trim().ToLowerInvariant();
    }

    private async Task<bool> HasPageAttachmentPositionColumnAsync()
    {
        if (!_db.Database.IsSqlite()) return true;

        var connection = _db.Database.GetDbConnection();
        var shouldClose = connection.State != System.Data.ConnectionState.Open;

        if (shouldClose)
            await connection.OpenAsync();

        try
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "PRAGMA table_info(PageAttachments);";
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                if (string.Equals(reader.GetString(1), "Position", StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
        finally
        {
            if (shouldClose)
                await connection.CloseAsync();
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PageResponseDto>>> GetAll([FromQuery] PageType? type)
    {
        var hasAttachmentPositionColumn = await HasPageAttachmentPositionColumnAsync();

        var query = _db.Pages
            .AsNoTracking()
            .Include(p => p.ResponsibleTeamMembers)
                .ThenInclude(m => m.Department)
            .OrderBy(p => p.Title)
            .AsQueryable();

        if (hasAttachmentPositionColumn)
            query = query.Include(p => p.Attachments);

        if (type.HasValue)
            query = query.Where(p => p.Type == type.Value);

        var items = await query.ToListAsync();
        return Ok(items.Select(MapPage));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PageResponseDto>> GetById(int id)
    {
        var hasAttachmentPositionColumn = await HasPageAttachmentPositionColumnAsync();

        var query = _db.Pages
            .AsNoTracking()
            .Include(p => p.ResponsibleTeamMembers)
                .ThenInclude(m => m.Department)
            .AsQueryable();

        if (hasAttachmentPositionColumn)
            query = query.Include(p => p.Attachments);

        var item = await query.FirstOrDefaultAsync(p => p.Id == id);
        if (item == null) return NotFound();

        return Ok(MapPage(item));
    }

    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<PageResponseDto>> GetBySlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return BadRequest("Slug is required.");

        var normalizedSlug = NormalizeSlug(slug);
        var hasAttachmentPositionColumn = await HasPageAttachmentPositionColumnAsync();

        var query = _db.Pages
            .AsNoTracking()
            .Include(p => p.ResponsibleTeamMembers)
                .ThenInclude(m => m.Department)
            .AsQueryable();

        if (hasAttachmentPositionColumn)
            query = query.Include(p => p.Attachments);

        var item = await query.FirstOrDefaultAsync(p => p.Slug == normalizedSlug);
        if (item == null) return NotFound();

        return Ok(MapPage(item));
    }

    [HttpPost]
    public async Task<ActionResult<PageResponseDto>> Create([FromBody] PageUpsertDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title)) return BadRequest("Title is required.");
        if (string.IsNullOrWhiteSpace(dto.Slug)) return BadRequest("Slug is required.");

        var page = new Page
        {
            Title = dto.Title.Trim(),
            Slug = NormalizeSlug(dto.Slug),
            Type = dto.Type,
            BodyHtml = dto.BodyHtml,
            CreatedAt = DateTime.UtcNow
        };

        if (dto.ResponsibleTeamMemberIds is { Count: > 0 })
        {
            var memberIds = dto.ResponsibleTeamMemberIds.Distinct().ToList();
            var members = await _db.TeamMembers
                .Where(m => memberIds.Contains(m.Id))
                .ToListAsync();

            foreach (var member in members)
            {
                page.ResponsibleTeamMembers.Add(member);
            }
        }

        _db.Pages.Add(page);
        await _db.SaveChangesAsync();

        var created = await _db.Pages
            .AsNoTracking()
            .Include(p => p.Attachments)
            .Include(p => p.ResponsibleTeamMembers)
                .ThenInclude(m => m.Department)
            .FirstAsync(p => p.Id == page.Id);

        return CreatedAtAction(nameof(GetById), new { id = page.Id }, MapPage(created));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PageUpsertDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title)) return BadRequest("Title is required.");
        if (string.IsNullOrWhiteSpace(dto.Slug)) return BadRequest("Slug is required.");

        var existing = await _db.Pages
            .Include(p => p.ResponsibleTeamMembers)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (existing is null) return NotFound();
        
        existing.Title = dto.Title.Trim();
        existing.Slug = NormalizeSlug(dto.Slug);
        existing.BodyHtml = dto.BodyHtml;
        existing.Type = dto.Type;

        if (dto.ResponsibleTeamMemberIds != null)
        {
            var memberIds = dto.ResponsibleTeamMemberIds.Distinct().ToList();
            var members = await _db.TeamMembers
                .Where(m => memberIds.Contains(m.Id))
                .ToListAsync();

            existing.ResponsibleTeamMembers.Clear();
            foreach (var member in members)
            {
                existing.ResponsibleTeamMembers.Add(member);
            }
        }

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

