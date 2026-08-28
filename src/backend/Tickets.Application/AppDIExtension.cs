using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tickets.Application.Handlers.EventEmailHandler;
using Tickets.Application.Interfaces;
using Tickets.Application.Services;
using Tickets.Application.UseCases.Auth;
using Tickets.Application.UseCases.Tickets;
using Tickets.Application.UseCases.Tickets.CreateTicket;
using Tickets.Application.UseCases.Tickets.GetTickets;
using Tickets.Application.UseCases.Users.ChangePassword;
using Tickets.Application.UseCases.Users.CreateUser;
using Tickets.Application.UseCases.Users.DeleteUser;
using Tickets.Application.UseCases.Users.ForgotPassword;
using Tickets.Application.UseCases.Users.GetUserById;
using Tickets.Application.UseCases.Users.GetUsers;
using Tickets.Application.UseCases.Users.UpdateUser;

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
            services.AddScoped<IUpdatePasswordUseCase, UpdatePasswordUseCase>();
            services.AddScoped<IForgotPasswordUseCase, ForgotPasswordUseCase>();
            services.AddScoped<IGetTicketsUseCase, GetTicketsUseCase>();
            services.AddScoped<ICreateTicketUseCase, CreateTicketUseCase>();

            // Handlers
            services.AddScoped<IEventEmailHandler, EventEmailHandler>();

            // Services
            services.AddScoped<IPasswordService, PasswordService>();

            // Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
