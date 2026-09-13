namespace Tickets.Domain.Entities
{
    public class TicketMessage : BaseEntity
    {
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
    }
}
