namespace Tickets.WebAPI.Models.Auth.Request
{
    public class ResetPasswordRequestModel
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
