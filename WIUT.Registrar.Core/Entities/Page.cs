namespace WIUT.Registrar.Core.Entities;

public enum PageType
{
    Admissions,
    Compliance,
    Policies,
    Records
}

public class Page : BaseEntity
{
    public required string Title { get; set; }
    public required string Slug { get; set; }
    public PageType Type { get; set; }
    public string? BodyHtml { get; set; }
    public ICollection<PageAttachment> Attachments { get; set; } = new List<PageAttachment>();
}


