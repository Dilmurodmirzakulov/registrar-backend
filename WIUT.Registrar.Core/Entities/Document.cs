namespace WIUT.Registrar.Core.Entities;

public enum DocumentCategory
{
    Handbook,
    ProgrammeSpec,
    Policy
}

public enum DocumentVisibility
{
    StudentOnly = 1,   // Only students
    StaffOnly = 2,     // Only staff
    Restricted = 3     // Only registrar/admin
}

public class Document : BaseEntity
{
    public required string FileName { get; set; }
    public required string FileUrl { get; set; }
    public long FileSize { get; set; }
    public int Position { get; set; }
    public DocumentCategory Category { get; set; }
    public int? Year { get; set; }
    public string? ProgrammeCode { get; set; }
    
    // New fields for visibility control
    public DocumentVisibility Visibility { get; set; } = DocumentVisibility.StudentOnly;
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }
}


