namespace Tickets.Domain.Entities
{
    public class UserPassword : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public string HashPassword { get; set; }
        public DateTime ExpirationDate { get; set; }

        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }

        public UserPassword()
        {
            
        }

    }
}
