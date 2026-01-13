namespace WIUT.Registrar.Core.Entities;

public class FAQ : BaseEntity
{
    public required string Question { get; set; }
    public required string Answer { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public string? Category { get; set; }
}







