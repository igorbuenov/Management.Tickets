using Tickets.Domain.Enums;

namespace Tickets.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketsPriority Priority { get; set; }
        public TicketStatus Status { get; set; }

        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }

        public int? AssignedToUserId { get; set; } = null;
        public User? AssignedToUser { get; set; }
    }
}
