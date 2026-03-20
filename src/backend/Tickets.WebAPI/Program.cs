using Tickets.Application;
using Tickets.Infrastructure;
using Tickets.WebAPI.Configuration;
using Tickets.WebAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersConfiguration()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddSwaggerConfiguration()
    .AddAutoMapperConfiguration();

var app = builder.Build();

app.UseSwaggerConfiguration();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
