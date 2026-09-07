namespace Tickets.Application.Interfaces
{
    public interface IPasswordResetTokenService
    {
        string GenerateToken();
        string HashToken(string token);
    }
}
