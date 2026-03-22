using System.ComponentModel.DataAnnotations;

namespace Tickets.WebAPI.Models.Users.Request
{
    public class CreateUserRequestModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
    }
}
