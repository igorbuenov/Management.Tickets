using System.Security.Cryptography;
using System.Text;
using Tickets.Application.Services.Interfaces;

namespace Tickets.Application.Services
{
    public class PasswordService : IPasswordService
    {
        public string SaltKey { get; set; }

        public string GenerateRandomPassword()
        {
            int length = 12;

            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            const string special = "!@#$%^&*()-_=+[]{}|;:,.<>?";

            string allChars = lower + upper + numbers + special;

            var passwordChars = new char[length];

            // Garantir pelo menos um de cada
            passwordChars[0] = GetRandomChar(lower);
            passwordChars[1] = GetRandomChar(upper);
            passwordChars[2] = GetRandomChar(numbers);
            passwordChars[3] = GetRandomChar(special);

            for (int i = 4; i < length; i++)
            {
                passwordChars[i] = GetRandomChar(allChars);
            }

            // Embaralhar a senha
            return new string(passwordChars.OrderBy(_ => RandomNumberGenerator.GetInt32(int.MaxValue)).ToArray());
        }

        private static char GetRandomChar(string chars)
        {
            int index = RandomNumberGenerator.GetInt32(chars.Length);
            return chars[index];
        }

        public string EncryptPassword(string password)
        {
            var newPassword = $"{password}{SaltKey}";

            var bytes = Encoding.UTF8.GetBytes(newPassword);
            var hashBytes = SHA512.HashData(bytes);

            return StringBytes(hashBytes);
        }

        private static string StringBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }



    }
}
