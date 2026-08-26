using System.ComponentModel.DataAnnotations;

namespace Tickets.WebAPI.Models.Users.Request
{
    public class ForgotPasswordUserRequestModel
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
    }
}
