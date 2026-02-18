using System.Text.Json.Serialization;

namespace WIUT.Registrar.Core.Entities;

public class PageAttachment : BaseEntity
{
    public int PageId { get; set; }
    public int Position { get; set; }
    public required string Title { get; set; }
    public string? Caption { get; set; }
    public required string FileUrl { get; set; }
    public required string FileName { get; set; }
    public long FileSize { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public bool IsImage { get; set; }

    [JsonIgnore]
    public Page? Page { get; set; }
}



