namespace Tickets.Application.Events.Tickets
{
    public class TicketMessageCreatedEvent
    {
        public int TicketId { get; set; }
        public int SenderUserId { get; set; }
    }
}
