using Microsoft.Extensions.DependencyInjection;
using Tickets.Application.Services;
using Tickets.Application.UseCases.Auth;
using Tickets.Application.UseCases.Users.CreateUser;
using Tickets.Application.UseCases.Users.GetUserById;
using Tickets.Application.UseCases.Users.GetUsers;
using Tickets.Application.UseCases.Users.UpdateUser;
using Tickets.Application.Validators.Users;
using FluentValidation;
using System.Reflection;
using Tickets.Application.UseCases.Users.DeleteUser;
using Tickets.Application.Interfaces;

namespace Tickets.Application
{
    public static class AppDIExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // UseCases
            services.AddScoped<IAuthenticateUserUseCase, AuthenticateUserUseCase>();
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<IGetUsersUseCase, GetUsersUseCase>();
            services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();

            // Services
            services.AddScoped<IPasswordService, PasswordService>();

            // Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
