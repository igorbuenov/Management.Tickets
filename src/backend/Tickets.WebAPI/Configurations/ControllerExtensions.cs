using Tickets.WebAPI.Filters;

namespace Tickets.WebAPI.Configurations
{
    public static class ControllerExtensions
    {
        public static IServiceCollection AddControllersConfiguration(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
            });

            return services;
        }
    }
}
