using Tickets.Domain.Enums;

namespace Tickets.WebAPI.Models.Tickets
{
    public class TicketModel
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public int CreatedByUserId { get; set; }
        public int? AssignedToUserId { get; set; }
    }
}
