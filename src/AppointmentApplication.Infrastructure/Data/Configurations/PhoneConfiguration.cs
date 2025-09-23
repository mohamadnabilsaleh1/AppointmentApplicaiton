using AppointmentApplication.Domain.Shared.Phone;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.OwnerType).IsRequired().HasMaxLength(50);
        builder.Property(e => e.OwnerId).IsRequired();
        builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(e => e.Label).HasMaxLength(50);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);

        builder.HasIndex(e => new { e.OwnerType, e.OwnerId });
    }
}
