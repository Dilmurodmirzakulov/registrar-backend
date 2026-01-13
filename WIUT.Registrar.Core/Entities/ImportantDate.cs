namespace WIUT.Registrar.Core.Entities;

public class ImportantDate : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public string SectionKey { get; set; } = "general";
    public string? LinkUrl { get; set; }
    public int? SortOrder { get; set; }
}









