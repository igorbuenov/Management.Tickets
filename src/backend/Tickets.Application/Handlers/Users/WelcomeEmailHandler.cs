using Tickets.Application.Events.Users;
using Tickets.Application.Interfaces;

namespace Tickets.Application.Handlers.Users
{
    public class WelcomeEmailHandler
    {
        private readonly IUserEmailService _userEmailService;

        public WelcomeEmailHandler(IUserEmailService userEmailService)
        {
            _userEmailService = userEmailService;
        }

        public async Task HandleAsync(
            CreateUserEmailEvent @event,
            CancellationToken cancellationToken)
        {
            await _userEmailService.SendWelcomeEmailAsync(
                @event.Email,
                @event.Name,
                @event.Password);
        }
    }
}
