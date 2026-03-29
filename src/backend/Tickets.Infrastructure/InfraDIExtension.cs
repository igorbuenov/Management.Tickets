using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Tickets.Application.Interfaces;
using Tickets.Application.Services.Interfaces;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;
using Tickets.Infrastructure.Identity;
using Tickets.Infrastructure.Repositories;
using Tickets.Infrastructure.Security;
using Tickets.Infrastructure.Settings;
using Tickets.Infrastructure.Services.Email;

namespace Tickets.Infrastructure
{
    public static class InfraDIExtension
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {

            // DbContext
            services.AddDbContext<TicketsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddHttpContextAccessor();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordRepository, PasswordRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICurrentUser, CurrentUser>();

            // Email Service
            services.Configure<BrevoSettings>(configuration.GetSection("BrevoSettings"));
            services.AddHttpClient<IEmailService, BrevoEmailService>();

            return services;
        }
    }
}
