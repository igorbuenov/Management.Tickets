using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tickets.Application.UseCases.Notifications.GetNotifications;
using Tickets.Application.UseCases.Notifications.MarkNotificationAsRead;

namespace Tickets.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IGetNotificationsUseCase _getNotificationsUseCase;
        private readonly IMarkNotificationAsReadUseCase _markNotificationAsReadUseCase;

        public NotificationsController(
            IGetNotificationsUseCase getNotificationsUseCase, IMarkNotificationAsReadUseCase markNotificationAsReadUseCase)
        {
            _getNotificationsUseCase = getNotificationsUseCase;
            _markNotificationAsReadUseCase = markNotificationAsReadUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var notifications = await _getNotificationsUseCase.Execute();
            return Ok(notifications);
        }

        [HttpPatch("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _markNotificationAsReadUseCase.Execute(id);
            return NoContent();
        }
    }
}