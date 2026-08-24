using Tickets.Application.Events.Users;

namespace Tickets.Application.Handlers.PasswordRecoveryEmail
{
    public interface IPasswordRecoveryEmailHandler
    {
        Task HandleAsync(PasswordRecoveryEmailEvent @event, CancellationToken cancellationToken);
    }
}
