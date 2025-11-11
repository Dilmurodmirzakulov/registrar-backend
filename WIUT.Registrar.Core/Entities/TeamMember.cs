namespace WIUT.Registrar.Core.Entities;

public class TeamMember : BaseEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? Title { get; set; }
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public string? PhotoUrl { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int DisplayOrder { get; set; }
}


