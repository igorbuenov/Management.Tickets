namespace Tickets.Application.Commons.Security
{
    public static class PasswordPolicy
    {
        public const int MinLength = 8;
        public const int MaxLength = 12;
        public const int PasswordHistoryLimit = 5;
        public const int ExpirationDays = 45;

        public const string AllowedSpecialCharacters = "!@#$%^&*()-_=+[]{}|;:,.<>?";

        public const string Regex =
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()\-_=\+\[\]{}|;:,.<>?])[A-Za-z\d!@#$%^&*()\-_=\+\[\]{}|;:,.<>?]{8,12}$";

        public static string DefaultValidationMessage =>
            $"Password must contain between {MinLength} and {MaxLength} characters, with at least one uppercase letter, one lowercase letter, one number, and one special character.";
    }
}
