namespace Tickets.WebAPI.Models.Departments.Response
{
    public class CreateDepartmentResponseModel
    {
        public bool Success { get; set; }
        public DepartmentModel Department { get; set; }
    }
}
