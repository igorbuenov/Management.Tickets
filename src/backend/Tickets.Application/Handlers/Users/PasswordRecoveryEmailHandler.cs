using Tickets.Application.Events.Users;
using Tickets.Application.Interfaces;

namespace Tickets.Application.Handlers.Users
{
    public class PasswordRecoveryEmailHandler
    {
        private readonly IUserEmailService _userEmailService;

        public PasswordRecoveryEmailHandler(IUserEmailService userEmailService)
        {
            _userEmailService = userEmailService;
        }

        public async Task HandleAsync(
            PasswordRecoveryEmailEvent @event,
            CancellationToken cancellationToken)
        {
            await _userEmailService.SendPasswordResetEmailAsync(
                @event.Email,
                @event.Name,
                @event.ResetLink);
        }
    }
}
