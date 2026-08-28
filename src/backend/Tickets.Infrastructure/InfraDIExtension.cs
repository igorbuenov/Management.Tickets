using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tickets.Application.Interfaces;
using Tickets.Application.Interfaces.Messaging;
using Tickets.Domain.Interfaces.Repositories;
using Tickets.Infrastructure.Data;
using Tickets.Infrastructure.Identity;
using Tickets.Infrastructure.Messaging;
using Tickets.Infrastructure.Repositories;
using Tickets.Infrastructure.Security.Jwt;
using Tickets.Infrastructure.Security.PasswordHashing;
using Tickets.Infrastructure.Services.BackgroundServices;
using Tickets.Infrastructure.Services.Email;
using Tickets.Infrastructure.Settings;

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
            services.AddScoped<IUserPasswordHistoryRepository, UserPasswordHistoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddScoped<IOutboxRepository, OutboxRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            // Password Hashing
            services.AddScoped<IPasswordHasher, BCryptPasswordHashAlgorithm>();

            // Messaging
            services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
            services.AddSingleton<IMessageConsumer, RabbitMqConsumer>();

            // Background Services
            services.AddHostedService<OutboxProcessor>();
            services.AddHostedService<RabbitMqConsumerService>();

            // Settings
            services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMqSettings"));
            services.Configure<BrevoSettings>(configuration.GetSection("BrevoSettings"));

            // Email Service
            services.AddHttpClient<IEmailService, BrevoEmailService>();
            services.AddScoped<IUserEmailService, UserEmailService>();

            return services;
        }
    }
}
