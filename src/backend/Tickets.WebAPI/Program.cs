using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using Tickets.Application;
using Tickets.Infrastructure;
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

app.UseRequestLoggingContext();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
