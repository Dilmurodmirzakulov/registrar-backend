namespace WIUT.Registrar.Core.Entities;

public class News : BaseEntity
{
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public string? Summary { get; set; }
    public string? BodyHtml { get; set; }
    public string? CoverImageUrl { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? TagsCsv { get; set; }
}


