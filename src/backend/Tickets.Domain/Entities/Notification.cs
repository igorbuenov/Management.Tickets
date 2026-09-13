namespace Tickets.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
    }
}
