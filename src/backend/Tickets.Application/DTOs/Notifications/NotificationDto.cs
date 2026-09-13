namespace Tickets.Application.DTOs.Notifications
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
