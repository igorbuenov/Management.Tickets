namespace Tickets.Application.Interfaces
{
    public interface IUserEmailService
    {
        Task SendWelcomeEmailAsync(string email, string name, string temporaryPassword);
    }
}
