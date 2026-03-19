using Tickets.Application;
using Tickets.Infrastructure;
using Tickets.WebAPI.Filters;
using Tickets.WebAPI.Mappings.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Filters Exceptions
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

// Dependency Injection
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);


// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(UserProfile));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Tickets WebAPI",
        Version = "v1",
        Description = "WebApi Sistema de Tickets"
    });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tickets WebAPI v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
