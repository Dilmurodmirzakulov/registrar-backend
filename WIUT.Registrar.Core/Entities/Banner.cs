namespace WIUT.Registrar.Core.Entities;

public class Banner : BaseEntity
{
    public required string ImageUrl { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public string? LinkUrl { get; set; }
    public string SectionKey { get; set; } = "general";
}

