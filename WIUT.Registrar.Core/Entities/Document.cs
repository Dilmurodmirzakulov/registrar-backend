namespace WIUT.Registrar.Core.Entities;

public enum DocumentCategory
{
    Handbook,
    ProgrammeSpec,
    Policy
}

public class Document : BaseEntity
{
    public required string FileName { get; set; }
    public required string FileUrl { get; set; }
    public long FileSize { get; set; }
    public DocumentCategory Category { get; set; }
    public int? Year { get; set; }
    public string? ProgrammeCode { get; set; }
}


