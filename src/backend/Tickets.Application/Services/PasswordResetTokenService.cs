using Tickets.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Tickets.Application.Services
{
    public class PasswordResetTokenService : IPasswordResetTokenService
    {
        public string GenerateToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(tokenBytes);
        }

        public string HashToken(string token)
        {
            var bytes = SHA256.HashData(
                Encoding.UTF8.GetBytes(token));

            return Convert.ToHexString(bytes);
        }
    }
}
