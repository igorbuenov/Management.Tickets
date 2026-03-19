using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Tickets.Infrastructure.Data
{
    public class TicketsDbContextFactory : IDesignTimeDbContextFactory<TicketsDbContext>
    {
        public TicketsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TicketsDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=DESKTOP-86U97VL\\SQLEXPRESS;Database=tickets;Trusted_Connection=True;TrustServerCertificate=True;");


            return new TicketsDbContext(optionsBuilder.Options);
        }
    }
}