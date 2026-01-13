using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WIUT.Registrar.Core.Entities;
using WIUT.Registrar.Infrastructure;

namespace WIUT.Registrar.Api.Controllers;

[ApiController]
[Route("api/pages/{pageId:int}/attachments")]
public class PageAttachmentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PageAttachmentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PageAttachmentResponse>>> List(int pageId)
    {
        var exists = await _db.Pages.AnyAsync(p => p.Id == pageId);
        if (!exists) return NotFound();

        var items = await _db.PageAttachments
            .Where(a => a.PageId == pageId)
            .OrderByDescending(a => a.CreatedAt)
            .Select(a => new PageAttachmentResponse(
                a.Id,
                a.PageId,
                a.Title,
                a.Caption,
                a.FileUrl,
                a.FileName,
                a.FileSize,
                a.ContentType,
                a.IsImage,
                a.CreatedAt,
                a.UpdatedAt
            ))
            .ToListAsync();
        return Ok(items);
    }

    public record AttachmentDto(
        string Title,
        string? Caption,
        string FileUrl,
        string FileName,
        long FileSize,
        string ContentType,
        bool IsImage);

    public record PageAttachmentResponse(
        int Id,
        int PageId,
        string Title,
        string? Caption,
        string FileUrl,
        string FileName,
        long FileSize,
        string ContentType,
        bool IsImage,
        DateTime CreatedAt,
        DateTime? UpdatedAt);

    [HttpPost]
    public async Task<ActionResult<PageAttachmentResponse>> Create(int pageId, [FromBody] AttachmentDto dto)
    {
        var page = await _db.Pages.FindAsync(pageId);
        if (page is null) return NotFound();

        var attachment = new PageAttachment
        {
            PageId = pageId,
            Title = dto.Title,
            Caption = dto.Caption,
            FileUrl = dto.FileUrl,
            FileName = dto.FileName,
            FileSize = dto.FileSize,
            ContentType = dto.ContentType,
            IsImage = dto.IsImage,
            CreatedAt = DateTime.UtcNow,
            IsPublished = true
        };

        _db.PageAttachments.Add(attachment);
        await _db.SaveChangesAsync();
        
        var response = new PageAttachmentResponse(
            attachment.Id,
            attachment.PageId,
            attachment.Title,
            attachment.Caption,
            attachment.FileUrl,
            attachment.FileName,
            attachment.FileSize,
            attachment.ContentType,
            attachment.IsImage,
            attachment.CreatedAt,
            attachment.UpdatedAt
        );
        
        return CreatedAtAction(nameof(List), new { pageId }, response);
    }

    [HttpDelete("{attachmentId:int}")]
    public async Task<IActionResult> Delete(int pageId, int attachmentId)
    {
        var attachment = await _db.PageAttachments
            .FirstOrDefaultAsync(a => a.PageId == pageId && a.Id == attachmentId);
        if (attachment is null) return NotFound();

        _db.PageAttachments.Remove(attachment);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}



