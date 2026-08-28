using Tickets.Application.DTOs.Tickets;

namespace Tickets.WebAPI.Models.Tickets.Response
{
    public class CreateTicketResponseModel
    {
        public bool Success { get; set; } = false;
        public TicketModel Ticket { get; set; }
    }
}
