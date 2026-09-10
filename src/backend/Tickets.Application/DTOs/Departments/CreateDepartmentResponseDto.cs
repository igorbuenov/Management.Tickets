namespace Tickets.Application.DTOs.Departments
{
    public class CreateDepartmentResponseDto
    {
        public bool Success { get; set; }
        public DepartmentDto Department { get; set; }
    }
}
