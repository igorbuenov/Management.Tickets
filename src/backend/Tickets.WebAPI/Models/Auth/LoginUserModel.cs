namespace Tickets.WebAPI.Models.Auth
{
    public class LoginUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<string> Roles { get; set; } = Array.Empty<string>();
    }
}
