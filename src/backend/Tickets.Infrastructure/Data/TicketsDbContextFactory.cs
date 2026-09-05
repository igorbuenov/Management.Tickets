using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tickets.Infrastructure.Data
{
    public class TicketsDbContextFactory : IDesignTimeDbContextFactory<TicketsDbContext>
    {
        public TicketsDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable(
                "ASPNETCORE_ENVIRONMENT") ?? "Production";

            var connectionString = environment switch
            {
                "Development" =>
                    "Server=DESKTOP-86U97VL\\SQLEXPRESS;Database=tickets;Trusted_Connection=True;TrustServerCertificate=True;",

                "Production" =>
                    Environment.GetEnvironmentVariable(
                        "Azure__ConnectionString"),

                _ => throw new InvalidOperationException(
                    $"Ambiente '{environment}' não possui uma connection string configurada.")
            };

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Connection string não configurada para o ambiente '{environment}'.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<TicketsDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new TicketsDbContext(optionsBuilder.Options);
        }
    }
}