namespace Tickets.Application.DTOs.Users
{
    public class GetUsersResponseDto
    {
        public bool Success { get; set; }
        public IEnumerable<UserDto> Users { get; set; }
    }
}
