using Tickets.Application.DTOs.Users;

namespace Tickets.WebAPI.Models.Users.Response
{
    public class CreateUserResponseModel
    {
        public bool Success { get; set; } = false;
        public CreateUserDto User { get; set; }
    }
}
