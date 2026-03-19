using Tickets.Domain.Entities;

namespace Tickets.Application.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(int userId, string email,IEnumerable<Role> roles);
    }
}
