using Tickets.Application.DTOs.Users;

namespace Tickets.Application.DTOs.Tickets
{
    public class TicketDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

        public UserSummaryDto CreatedBy { get; set; }
        public UserSummaryDto? AssignedTo { get; set; }
    }
}
