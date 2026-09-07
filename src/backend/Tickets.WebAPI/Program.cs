using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using Tickets.Application;
using Tickets.Infrastructure;
using Tickets.Infrastructure.Data;
using Tickets.Infrastructure.Settings;
using Tickets.WebAPI.Configuration;
using Tickets.WebAPI.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    var columnOptions = new ColumnOptions();

    columnOptions.AdditionalColumns = new List<SqlColumn>
    {
        new SqlColumn { ColumnName = "UserId", DataType = SqlDbType.NVarChar, DataLength = 50 },
        new SqlColumn { ColumnName = "RequestPath", DataType = SqlDbType.NVarChar, DataLength = 500 },
        new SqlColumn { ColumnName = "RequestMethod", DataType = SqlDbType.NVarChar, DataLength = 20 },
        new SqlColumn { ColumnName = "ClientIp", DataType = SqlDbType.NVarChar, DataLength = 50 },
        new SqlColumn { ColumnName = "RequestId", DataType = SqlDbType.NVarChar, DataLength = 100 }
    };

    configuration
        .MinimumLevel.Information()
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .Enrich.WithEnvironmentName()
        .WriteTo.MSSqlServer(
            connectionString: context.Configuration.GetConnectionString("DefaultConnection"),
            sinkOptions: new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = true
            },
            columnOptions: columnOptions
        );
});

builder.Services
    .AddControllersConfiguration()
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddJwtAuthentication(builder.Configuration)
    .AddSwaggerConfiguration()
    .AddAutoMapperConfiguration()
    .AddCors(options =>
    {
        options.AddPolicy("Angular", policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:4200",
                    "https://management-tickets-front-end.vercel.app")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<TicketsDbContext>();

    dbContext.Database.Migrate();
}

app.UseSwaggerConfiguration();

app.UseHttpsRedirection();

app.UseCors("Angular");

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestLoggingContext();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
