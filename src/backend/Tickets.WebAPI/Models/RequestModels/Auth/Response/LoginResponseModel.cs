namespace Tickets.WebAPI.Models.RequestModels.Auth.Response
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
