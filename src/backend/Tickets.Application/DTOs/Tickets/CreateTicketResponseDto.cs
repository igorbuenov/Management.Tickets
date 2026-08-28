namespace Tickets.Application.DTOs.Tickets
{
    public class CreateTicketResponseDto
    {
        public bool Success { get; set; } = false;
        public TicketDto Ticket { get; set; }
    }
}
