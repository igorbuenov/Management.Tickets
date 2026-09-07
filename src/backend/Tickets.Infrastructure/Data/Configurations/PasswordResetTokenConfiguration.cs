using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickets.Domain.Entities;

namespace Tickets.Infrastructure.Data.Configurations
{
    public class PasswordResetTokenConfiguration : BaseEntityConfiguration<PasswordResetToken>
    {
        public override void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {
            base.Configure(builder);

            builder.ToTable("PasswordResetTokens");

            builder.Property(x => x.TokenHash)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(x => x.ExpiresAt)
                .IsRequired();

            builder.Property(x => x.UsedAt)
                .IsRequired(false);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.HasIndex(x => x.TokenHash)
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}