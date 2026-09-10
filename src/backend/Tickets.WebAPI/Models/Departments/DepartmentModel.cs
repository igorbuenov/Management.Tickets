namespace Tickets.WebAPI.Models.Departments
{
    public class DepartmentModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Name { get; set; }
    }
}
