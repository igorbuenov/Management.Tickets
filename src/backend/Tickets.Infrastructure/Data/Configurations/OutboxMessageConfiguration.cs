using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickets.Domain.Entities;

namespace Tickets.Infrastructure.Data.Configurations
{
    public class OutboxMessageConfiguration : BaseEntityConfiguration<OutboxMessage>
    {
        public override void Configure(
        EntityTypeBuilder<OutboxMessage> builder)
        {
            base.Configure(builder);

            builder.ToTable("OutboxMessages");

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Content)
                .IsRequired();

            builder.Property(x => x.Error)
                .HasMaxLength(2000);
        }
    }
}
