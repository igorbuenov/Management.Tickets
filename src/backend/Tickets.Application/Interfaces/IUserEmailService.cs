using Tickets.Infrastructure.Services.Email.Enums;

namespace Tickets.Application.Interfaces
{
    public interface IUserEmailService
    {
        Task SendWelcomeEmailAsync(string email, string name, string temporaryPassword);
        Task SendPasswordResetEmailAsync(string email, string name, string resetLink);
    }
}
