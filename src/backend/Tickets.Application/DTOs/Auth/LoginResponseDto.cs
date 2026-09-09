using Tickets.Domain.Entities;

namespace Tickets.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool MustChangePassword { get; set; }
        public LoginUserDto User { get; set; }
    }
}
