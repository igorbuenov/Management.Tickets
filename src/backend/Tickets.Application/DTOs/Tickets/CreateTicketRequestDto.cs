using Tickets.Domain.Enums;

namespace Tickets.Application.DTOs.Tickets
{
    public class CreateTicketRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketsPriority Priority { get; set; }
        public int CategoryId { get; set; }
        public int DepartmentId { get; set; }
    }
}
