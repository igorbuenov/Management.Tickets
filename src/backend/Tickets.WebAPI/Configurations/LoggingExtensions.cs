using Tickets.WebAPI.Logging;

namespace Tickets.WebAPI.Configurations
{
    public static class LoggingExtensions
    {
        public static IApplicationBuilder UseRequestLoggingContext(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
