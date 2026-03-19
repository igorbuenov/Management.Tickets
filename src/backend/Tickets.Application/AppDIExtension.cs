using Microsoft.Extensions.DependencyInjection;
using Tickets.Application.Services;
using Tickets.Application.Services.Interfaces;
using Tickets.Application.UseCases.Auth;
using Tickets.Application.UseCases.Users.CreateUser;

namespace Tickets.Application
{
    public static class AppDIExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<IAuthenticateUserUseCase, AuthenticateUserUseCase>();
            services.AddScoped<IPasswordService, PasswordService>();

            return services;
        }
    }
}
