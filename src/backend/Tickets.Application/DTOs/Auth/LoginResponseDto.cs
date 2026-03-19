namespace Tickets.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
