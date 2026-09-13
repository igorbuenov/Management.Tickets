using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickets.Domain.Entities;

namespace Tickets.Infrastructure.Data.Configurations
{
    public class TicketMessageConfiguration : BaseEntityConfiguration<TicketMessage>
    {
        public override void Configure(EntityTypeBuilder<TicketMessage> builder)
        {
            base.Configure(builder);

            builder.ToTable("TicketMessages");

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Ticket)
                .WithMany()
                .HasForeignKey(x => x.TicketId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Message)
                   .IsRequired()
                   .HasMaxLength(2000);
        }
    }
}
