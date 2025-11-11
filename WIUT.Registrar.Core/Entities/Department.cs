namespace WIUT.Registrar.Core.Entities;

public class Department : BaseEntity
{
    public required string Name { get; set; }
    public int? ParentDepartmentId { get; set; }
    public Department? ParentDepartment { get; set; }
    public ICollection<Department> Children { get; set; } = new List<Department>();
}


