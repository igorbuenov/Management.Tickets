using Tickets.Application.DTOs.Auth;

namespace Tickets.WebAPI.Models.Auth.Response
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public LoginUserModel User { get; set; }
    }
}
