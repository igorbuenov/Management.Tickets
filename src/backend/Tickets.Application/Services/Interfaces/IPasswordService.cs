namespace Tickets.Application.Services.Interfaces
{
    public interface IPasswordService
    {
        string GenerateRandomPassword();
        string EncryptPassword(string password);
    }
}
