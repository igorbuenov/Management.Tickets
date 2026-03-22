namespace Tickets.Application.DTOs.Users
{
    public class CreateUserResponseDto
    {
        public bool Success { get; set; } = false;
        public CreateUserDto User { get; set; }
    }
}
