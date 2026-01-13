namespace WIUT.Registrar.Core.Entities;

public class SiteSetting : BaseEntity
{
    public required string Key { get; set; }
    public string? Value { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
}









