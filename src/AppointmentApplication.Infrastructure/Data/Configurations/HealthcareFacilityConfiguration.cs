// HealthcareFacilityConfiguration.cs
using AppointmentApplication.Domain.HealthcareFacilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;
    public class HealthcareFacilityConfiguration : IEntityTypeConfiguration<HealthCareFacility>
    {
        public void Configure(EntityTypeBuilder<HealthCareFacility> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
            builder.Property(e => e.Type).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Address).IsRequired();
            builder.Property(e => e.GPSLatitude).HasColumnType("decimal(9,6)");
            builder.Property(e => e.GPSLongitude).HasColumnType("decimal(9,6)");
            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.UpdatedAtdUtc);

            builder.HasQueryFilter(e => e.IsActive);
        }
    }
