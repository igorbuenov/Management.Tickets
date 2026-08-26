namespace Tickets.Domain.Entities
{
    public class OutboxMessage : BaseEntity
    {
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime? ProcessedAt { get; set; } = null;
        public int RetryCount { get; set; }
        public string? Error { get; set; }
    }
}
