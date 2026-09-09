namespace Tickets.WebAPI.Models.Users.Request
{
    public class ChangeTemporaryPasswordRequestModel
    {
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
