using Tickets.WebAPI.Models.Categories;
using Tickets.WebAPI.Models.Departments;
using Tickets.WebAPI.Models.Users;

namespace Tickets.WebAPI.Models.Tickets
{
    public class TicketModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public CategoryModel Category { get; set; }
        public DepartmentModel Department { get; set; }
        public UserSummaryModel CreatedBy { get; set; }
        public UserSummaryModel? AssignedTo { get; set; }
    }
}
