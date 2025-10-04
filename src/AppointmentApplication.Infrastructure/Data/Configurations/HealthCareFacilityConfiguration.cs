using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Users;

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

        // Configure the UserId property
        builder.Property(e => e.UserId).IsRequired();

        // Configure the relationship explicitly
        builder.HasOne(e => e.User)
            .WithMany(u => u.HealthCareFacilities)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.ClientSetNull);

        // Remove this line: builder.Ignore(e => e.UserId1);

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
        builder.HasMany(e => e.Schedules)
       .WithOne(s => s.Facility)
       .HasForeignKey(s => s.FacilityId)
       .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.ScheduleExceptions)
               .WithOne(se => se.Facility)
               .HasForeignKey(se => se.FacilityId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(e => e.IsActive);
        builder.HasMany(f => f.Doctors)
       .WithOne(d => d.HealthcareFacility)
       .HasForeignKey(d => d.FacilityId)
       .OnDelete(DeleteBehavior.Cascade);
    }
}