using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tickets.Application.DTOs.Auth;
using Tickets.Application.Interfaces;
using Tickets.Domain.Entities;

namespace Tickets.Infrastructure.Security.Jwt
{
    public class JwtService : IJwtService
    {

        private readonly JwtSettings _settings;

        public JwtService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        public JsonTokenResultDto GenerateToken(int userId, string email, IEnumerable<Role> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Description));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new JsonTokenResultDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt
            };
        }
    }
}
