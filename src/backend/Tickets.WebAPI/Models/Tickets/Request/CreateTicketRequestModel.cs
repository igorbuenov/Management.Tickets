using Tickets.Domain.Enums;

namespace Tickets.WebAPI.Models.Tickets.Request
{
    public class CreateTicketRequestModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TicketsPriority Priority { get; set; }
    }
}
