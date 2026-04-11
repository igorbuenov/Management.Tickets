using System.Security.Cryptography;
using Tickets.Application.Commons.Security;
using Tickets.Application.Interfaces;

namespace Tickets.Application.Services
{
    public class PasswordService : IPasswordService
    {

        private readonly IPasswordHasher _passwordHasher;

        public PasswordService(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string GenerateRandomPassword()
        {
            int length = RandomNumberGenerator.GetInt32(
                PasswordPolicy.MinLength,
                PasswordPolicy.MaxLength + 1);

            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            const string special = PasswordPolicy.AllowedSpecialCharacters;

            string allChars = lower + upper + numbers + special;

            var passwordChars = new char[length];

            passwordChars[0] = GetRandomChar(lower);
            passwordChars[1] = GetRandomChar(upper);
            passwordChars[2] = GetRandomChar(numbers);
            passwordChars[3] = GetRandomChar(special);

            for (int i = 4; i < length; i++)
            {
                passwordChars[i] = GetRandomChar(allChars);
            }

            return new string(passwordChars
                .OrderBy(_ => RandomNumberGenerator.GetInt32(int.MaxValue))
                .ToArray());
        }

        private static char GetRandomChar(string chars)
        {
            int index = RandomNumberGenerator.GetInt32(chars.Length);
            return chars[index];
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashPassword)
        {
            return _passwordHasher.VerifyPassword(password, hashPassword);
        }



    }
}
