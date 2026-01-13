namespace WIUT.Registrar.Core.Entities;

public class Statistic : BaseEntity
{
    public required string CardTitle { get; set; }
    public required string StatName { get; set; }
    public required string StatValue { get; set; }
    public string? Description { get; set; }
    public int DisplayOrder { get; set; } = 0;
}







