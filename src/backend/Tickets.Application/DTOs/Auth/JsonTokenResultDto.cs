namespace Tickets.Application.DTOs.Auth
{
    public class JsonTokenResultDto
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
