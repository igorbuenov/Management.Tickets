using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Tickets.Application.Interfaces;

namespace Tickets.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId 
        { 
            get
            {
                var userId = _httpContextAccessor
                    .HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                if (int.TryParse(userId, out var id))
                    return id;

                return null;

            }
        }
        public string? Email =>
            _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst(ClaimTypes.Email)?
                .Value;
    }
}
