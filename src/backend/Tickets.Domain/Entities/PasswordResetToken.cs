namespace Tickets.Domain.Entities
{
    public class PasswordResetToken : BaseEntity
    {
        public string TokenHash { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public DateTime? UsedAt { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } = null!;
    }
}
