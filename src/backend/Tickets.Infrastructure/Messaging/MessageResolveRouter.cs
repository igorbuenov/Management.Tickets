using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Application.Events.Tickets;
using Tickets.Application.Events.Users;
using Tickets.Application.Interfaces.Messaging;

namespace Tickets.Infrastructure.Messaging
{
    public class MessageResolveRouter : IMessageResolveRouter
    {
        public string Resolve(string messageType)
        {
            return messageType switch
            {
                nameof(CreateUserEmailEvent)
                    => MessagingQueues.WelcomeEmail,

                nameof(PasswordRecoveryEmailEvent)
                    => MessagingQueues.PasswordRecoveryEmail,

                nameof(TicketMessageCreatedEvent)
                    => MessagingQueues.TicketNotifications,

                _ => throw new InvalidOperationException(
                    $"Unknown message type: {messageType}")
            };
        }
    }
}
