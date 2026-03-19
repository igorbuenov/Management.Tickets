using Microsoft.EntityFrameworkCore;
using Tickets.Domain.Entities;

namespace Tickets.Infrastructure.Data
{
    public class TicketsDbContext : DbContext
    {

        public TicketsDbContext(DbContextOptions<TicketsDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserPassword> UserPasswords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica todas as configurações das entidades
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketsDbContext).Assembly);

            // Seed do usuário system
            modelBuilder.Entity<User>().HasData(
                new
                {
                    Id = 1,
                    Name = "System",
                    Email = "system@tickets.local",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 1, 1),
                    UpdatedAt = (DateTime?)null,
                    CreatedByUserId = (int?)null
                }
            );
        }

    }
}
