using System.ComponentModel.DataAnnotations;

namespace Tickets.Application.DTOs.Users
{
    public class CreateUserRequestDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
    }
}
