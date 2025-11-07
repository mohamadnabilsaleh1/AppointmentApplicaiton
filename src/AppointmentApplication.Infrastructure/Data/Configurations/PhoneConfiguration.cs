using AppointmentApplication.Domain.Shared;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id);

        builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(e => e.Label).HasMaxLength(50);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtUtc);
        builder.Property(e => e.UserId).IsRequired();
        builder.HasIndex(p => new { p.UserId, p.IsPrimary })
             .HasFilter("[IsPrimary] = 1")
             .IsUnique();

    }
}
