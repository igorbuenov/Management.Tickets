using Tickets.Application.Events.Users;
using Tickets.Application.Interfaces;

namespace Tickets.Application.Handlers.EventEmailHandler
{
    public class EventEmailHandler : IEventEmailHandler
    {
        private readonly IUserEmailService _userEmailService;

        public EventEmailHandler(IUserEmailService userEmailService)
        {
            _userEmailService = userEmailService;
        }

        public async Task HandleEventAsync(string type, object content, CancellationToken cancellationToken)
        {
            switch (type)
            {
                case nameof(CreateUserEmailEvent):
                    var welcomeEvent = content as CreateUserEmailEvent;
                    if (welcomeEvent == null)
                        throw new ArgumentException("Invalid content type for CreateUserEmailEvent.", nameof(content));
                    await _userEmailService.SendWelcomeEmailAsync(
                        welcomeEvent.Email,
                        welcomeEvent.Name,
                        welcomeEvent.Password);
                    break;

                case nameof(PasswordRecoveryEmailEvent):
                    var passwordRecoverEvent = content as PasswordRecoveryEmailEvent;
                    if (passwordRecoverEvent == null)
                        throw new ArgumentException("Invalid content type for PasswordRecoveryEmailEvent.", nameof(content));
                    await _userEmailService.SendPasswordResetEmailAsync(
                        passwordRecoverEvent.Email,
                        passwordRecoverEvent.Name,
                        passwordRecoverEvent.TemporaryPassword);
                    break;

                default:
                    throw new NotSupportedException($"Event type '{type}' is not supported.");
            }
        }
    }
}
