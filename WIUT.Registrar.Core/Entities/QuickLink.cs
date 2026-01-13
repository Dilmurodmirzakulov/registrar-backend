namespace WIUT.Registrar.Core.Entities;

public class QuickLink : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public required string LinkUrl { get; set; }
    public string IconKey { get; set; } = "default";
    public string ThemeKey { get; set; } = "default";
    public int DisplayOrder { get; set; } = 0;
    public bool IsExternal { get; set; } = false;
}









