using Tickets.WebAPI.Mappings.Users;
using Tickets.WebAPI.Mappings.Auth;

namespace Tickets.WebAPI.Configurations
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(UserProfile), typeof(AuthProfile));
            return services;
        }
    }
}
