namespace Tickets.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }                            
        public bool IsActive { get; set; } = true;
        public int? CreatedByUserId { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public User() { }
        

        public void Deactivate()
        {
            if (!IsActive)
                return;

            IsActive = false;
        }

        public void Activate()
        {
            if (IsActive)
                return;

            IsActive = true;
        }

        public void Update(string name, string email)
        {
            Name = name;
            Email = email;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
