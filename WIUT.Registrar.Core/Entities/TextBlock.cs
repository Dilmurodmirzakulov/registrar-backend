namespace WIUT.Registrar.Core.Entities;

public class TextBlock : BaseEntity
{
    public required string Title { get; set; }
    public required string Content { get; set; } // HTML content
    public PageType? PageType { get; set; } // Optional: associate with specific page type
    public string? SectionKey { get; set; } // Optional: associate with specific section (e.g., "home", "admissions")
    public int DisplayOrder { get; set; } = 0;
    public string? CssClass { get; set; } // Optional: for custom styling
}




