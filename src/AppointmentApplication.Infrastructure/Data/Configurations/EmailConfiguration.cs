using AppointmentApplication.Domain.Shared;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

// Contact Information Configurations
public class EmailConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id);

        builder.Property(e => e.EmailAddress).IsRequired().HasMaxLength(255);
        builder.Property(e => e.Label).HasMaxLength(50);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtUtc);
        builder.HasIndex(e => new { e.UserId, e.IsPrimary })
    .HasFilter("[IsPrimary] = 1")
    .IsUnique();

        // Index for faster queries by user
        builder.HasIndex(e => e.UserId);

    }
}
