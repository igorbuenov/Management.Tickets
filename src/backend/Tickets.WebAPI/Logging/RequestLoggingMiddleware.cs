using Serilog.Context;
using System.Security.Claims;

namespace Tickets.WebAPI.Logging
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            using (LogContext.PushProperty("UserId", userId ?? "Anonymous"))
            using (LogContext.PushProperty("RequestPath", context.Request.Path.Value))
            using (LogContext.PushProperty("RequestMethod", context.Request.Method))
            using (LogContext.PushProperty("ClientIp", context.Connection.RemoteIpAddress?.ToString()))
            using (LogContext.PushProperty("RequestId", context.TraceIdentifier))
            {
                await _next(context);
            }
        }


    }
}