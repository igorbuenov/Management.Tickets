using Tickets.Domain.Enums;

namespace Tickets.Application.DTOs.Tickets
{
    public class UpdateTicketStatusRequestDto
    {
        public TicketStatus Status { get; set; }
    }
}
