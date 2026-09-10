using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tickets.Application.Configurations;
using Tickets.Application.Handlers.EventEmailHandler;
using Tickets.Application.Interfaces;
using Tickets.Application.Services;
using Tickets.Application.UseCases.Auth.ForgotPassword;
using Tickets.Application.UseCases.Auth.ResetPassword;
using Tickets.Application.UseCases.Auth.UserLogin;
using Tickets.Application.UseCases.Categories.CreateCategory;
using Tickets.Application.UseCases.Categories.GetCategories;
using Tickets.Application.UseCases.Departments.CreateDepartment;
using Tickets.Application.UseCases.Departments.GetDepartments;
using Tickets.Application.UseCases.Departments.GetMyDepartments;
using Tickets.Application.UseCases.Tickets;
using Tickets.Application.UseCases.Tickets.CreateTicket;
using Tickets.Application.UseCases.Tickets.GetTicketById;
using Tickets.Application.UseCases.Tickets.GetTickets;
using Tickets.Application.UseCases.Users.ActiveUser;
using Tickets.Application.UseCases.Users.ChangePassword;
using Tickets.Application.UseCases.Users.ChangeTemporaryPassword;
using Tickets.Application.UseCases.Users.CreateUser;
using Tickets.Application.UseCases.Users.DeleteUser;
using Tickets.Application.UseCases.Users.GetUserById;
using Tickets.Application.UseCases.Users.GetUsers;
using Tickets.Application.UseCases.Users.UpdateUser;

namespace Tickets.Application
{
    public static class AppDIExtension
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // UseCases
            services.AddScoped<IAuthenticateUserUseCase, AuthenticateUserUseCase>();
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<IGetUsersUseCase, GetUsersUseCase>();
            services.AddScoped<IGetUserByIdUseCase, GetUserByIdUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IActiveUserUseCase, ActiveUserUseCase>();
            services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
            services.AddScoped<IUpdatePasswordUseCase, UpdatePasswordUseCase>();
            services.AddScoped<IChangeTemporaryPasswordUseCase, ChangeTemporaryPasswordUseCase>();
            services.AddScoped<IForgotPasswordUseCase, ForgotPasswordUseCase>();
            services.AddScoped<IResetPasswordUseCase, ResetPasswordUseCase>();
            services.AddScoped<IGetTicketsUseCase, GetTicketsUseCase>();
            services.AddScoped<ICreateTicketUseCase, CreateTicketUseCase>();
            services.AddScoped<IGetTicketByIdUseCase, GetTicketByIdUseCase>();
            services.AddScoped<ICreateDepartmentUseCase, CreateDepartmentUseCase>();
            services.AddScoped<IGetDepartmentsUseCase, GetDepartmentsUseCase>();
            services.AddScoped<ICreateCategorytUseCase, CreateCategoryUseCase>();
            services.AddScoped<IGetCategoriesUseCase, GetCategoriesUseCase>();
            services.AddScoped<IGetMyDepartmentsUseCase, GetMyDepartmentsUseCase>();


            // Handlers
            services.AddScoped<IEventEmailHandler, EventEmailHandler>();

            // Services
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IPasswordResetTokenService, PasswordResetTokenService>();

            // Settings
            services.Configure<FrontendSettings>(
                configuration.GetSection("FrontendSettings"));

            // Validators
            services.AddValidatorsFromAssembly(
                Assembly.GetExecutingAssembly());

            return services;
        }
    }
}