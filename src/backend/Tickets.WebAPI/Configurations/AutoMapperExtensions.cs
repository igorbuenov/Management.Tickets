using Tickets.WebAPI.Mappings.Users;

namespace Tickets.WebAPI.Configurations
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(UserProfile));
            return services;
        }
    }
}
