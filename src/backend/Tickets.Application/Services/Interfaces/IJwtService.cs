using Tickets.Application.DTOs.Auth;
using Tickets.Domain.Entities;

namespace Tickets.Application.Services.Interfaces
{
    public interface IJwtService
    {
        JsonTokenResultDto GenerateToken(int userId, string email,IEnumerable<Role> roles);
    }
}
