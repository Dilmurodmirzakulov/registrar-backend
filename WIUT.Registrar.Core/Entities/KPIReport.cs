namespace WIUT.Registrar.Core.Entities;

public class KPIReport : BaseEntity
{
    public required string Title { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public string? BodyHtml { get; set; }
    public string? AttachmentUrl { get; set; }
}


