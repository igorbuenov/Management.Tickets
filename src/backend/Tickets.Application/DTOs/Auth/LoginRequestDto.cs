namespace Tickets.Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }
}
