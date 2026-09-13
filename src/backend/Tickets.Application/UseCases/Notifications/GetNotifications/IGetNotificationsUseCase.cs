using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Application.DTOs.Notifications;

namespace Tickets.Application.UseCases.Notifications.GetNotifications
{
    public interface IGetNotificationsUseCase
    {
        Task<IEnumerable<NotificationDto>> Execute();
    }
}
