using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Tickets.Application.Interfaces;

namespace Tickets.Infrastructure.Identity
{
    public class CurrentUser : ICurrentUser
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public int? UserId 
        { 
            get
            {
                var userId = User?
                     .FindFirst(ClaimTypes.NameIdentifier)?
                     .Value;

                return int.TryParse(userId, out var id) ? id : null;

            }
        }

        public string? Email =>
            User?
                .FindFirst(ClaimTypes.Email)?
                .Value;

        public string? Role =>
            User?
                .FindFirst(ClaimTypes.Role)?
                .Value;

        public bool IsAuthenticated =>
            User?.Identity?.IsAuthenticated ?? false;
    }
}
