using Microsoft.Extensions.DependencyInjection;
using Tickets.Application.Services;
using Tickets.Application.Services.Interfaces;
using Tickets.Application.UseCases.Auth;
using Tickets.Application.UseCases.Users.CreateUser;
using Tickets.Application.UseCases.Users.GetUserById;
using Tickets.Application.UseCases.Users.GetUsers;
using Tickets.Application.UseCases.Users.UpdateUser;
using Tickets.Application.Validators.Users;
using FluentValidation;
using System.Reflection;

namespace Tickets.Application
{
    public static class AppDIExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // UseCases
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<IAuthenticateUserUseCase, AuthenticateUserUseCase>();
            services.AddScoped<IGetUsersUseCase, GetUsersUseCase>();
            services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();

            // Services
            services.AddScoped<IPasswordService, PasswordService>();

            // Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
