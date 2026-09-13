using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Tickets.Application.Events.Tickets;
using Tickets.Application.Events.Users;
using Tickets.Application.Handlers.Tickets;
using Tickets.Application.Handlers.Users;
using Tickets.Application.Interfaces.Messaging;

namespace Tickets.Infrastructure.Messaging
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EventDispatcher(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task DispatchAsync(
            string eventType,
            string content,
            CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();

            switch (eventType)
            {
                case nameof(CreateUserEmailEvent):
                    var welcomeEvent =
                        JsonSerializer.Deserialize<CreateUserEmailEvent>(content);

                    if (welcomeEvent is null)
                        throw new InvalidOperationException($"Could not deserialize event {eventType}.");

                    var welcomeHandler =
                        scope.ServiceProvider.GetRequiredService<WelcomeEmailHandler>();

                    await welcomeHandler.HandleAsync(
                        welcomeEvent,
                        cancellationToken);

                    break;

                case nameof(PasswordRecoveryEmailEvent):
                    var passwordRecoveryEvent =
                        JsonSerializer.Deserialize<PasswordRecoveryEmailEvent>(content);

                    if (passwordRecoveryEvent is null)
                        throw new InvalidOperationException($"Could not deserialize event {eventType}.");

                    var passwordRecoveryHandler =
                        scope.ServiceProvider.GetRequiredService<PasswordRecoveryEmailHandler>();

                    await passwordRecoveryHandler.HandleAsync(
                        passwordRecoveryEvent,
                        cancellationToken);

                    break;

                case nameof(TicketMessageCreatedEvent):
                    var ticketMessageEvent = 
                        JsonSerializer.Deserialize<TicketMessageCreatedEvent>(content);

                    if (ticketMessageEvent is null)
                        throw new InvalidOperationException($"Could not deserialize event {eventType}.");

                    var ticketMessageHandler =
                        scope.ServiceProvider.GetRequiredService<TicketMessageCreatedHandler>();

                    await ticketMessageHandler.HandleAsync(
                        ticketMessageEvent,
                        cancellationToken);

                    break;

                default:
                    throw new NotSupportedException($"Event type '{eventType}' is not supported.");
            }
        }
    }
}