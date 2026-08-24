using Tickets.Application.Events.Users;
using Tickets.Application.Interfaces;

namespace Tickets.Application.Handlers.PasswordRecoveryEmail
{
    internal class PasswordRecoveryEmailHandler : IPasswordRecoveryEmailHandler
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
                @event.TemporaryPassword);
        }
    }
}
