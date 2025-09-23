using AppointmentApplication.Domain.HealthcareFacilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

// Healthcare Facility Configurations
public class HealthCareFacilityConfiguration : IEntityTypeConfiguration<HealthCareFacility>
{
    public void Configure(EntityTypeBuilder<HealthCareFacility> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
        builder.Property(e => e.Type).IsRequired().HasMaxLength(100);
        builder.OwnsOne(e => e.Address, address =>
        {
            address.Property(a => a.Street).IsRequired().HasMaxLength(200);
            address.Property(a => a.City).IsRequired().HasMaxLength(100);
            address.Property(a => a.State).IsRequired().HasMaxLength(100);
            address.Property(a => a.Country).IsRequired().HasMaxLength(100);
            address.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
        });
        builder.Property(e => e.GPSLatitude).HasColumnType("decimal(9,6)");
        builder.Property(e => e.GPSLongitude).HasColumnType("decimal(9,6)");
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);
        
        builder.HasQueryFilter(e => e.IsActive);
    }
}
