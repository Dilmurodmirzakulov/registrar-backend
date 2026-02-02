namespace WIUT.Registrar.Core.Entities;

public enum DocumentCategory
{
    Handbook,
    ProgrammeSpec,
    Policy
}

public enum DocumentVisibility
{
    Public,        // Everyone can see
    StudentOnly,   // Only students
    StaffOnly,     // Only staff
    Restricted     // Only registrar/admin
}

public class Document : BaseEntity
{
    public required string FileName { get; set; }
    public required string FileUrl { get; set; }
    public long FileSize { get; set; }
    public DocumentCategory Category { get; set; }
    public int? Year { get; set; }
    public string? ProgrammeCode { get; set; }
    
    // New fields for visibility control
    public DocumentVisibility Visibility { get; set; } = DocumentVisibility.Public;
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }
}


