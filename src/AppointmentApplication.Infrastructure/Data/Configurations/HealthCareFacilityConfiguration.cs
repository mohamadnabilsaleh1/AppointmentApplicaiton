using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class HealthCareFacilityConfiguration : IEntityTypeConfiguration<HealthCareFacility>
{
    public void Configure(EntityTypeBuilder<HealthCareFacility> builder)
    {
        builder.ToTable("HealthCareFacilities"); // Explicit table name

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion<string>() // Convert enum to string
            .HasMaxLength(100);

        // UserId configuration - FIXED
        builder.Property(e => e.UserId)
            .IsRequired();

        // User relationship - FIXED (Remove ClientSetNull)
        builder.HasOne<Domain.Users.User>() // Use full namespace if needed
            .WithMany(u => u.HealthCareFacilities)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Changed from ClientSetNull to Restrict or Cascade

        // Address configuration
        builder.OwnsOne(e => e.Address, address =>
        {
            address.Property(a => a.Street).IsRequired().HasMaxLength(200);
            address.Property(a => a.City).IsRequired().HasMaxLength(100);
            address.Property(a => a.State).IsRequired().HasMaxLength(100);
            address.Property(a => a.Country).IsRequired().HasMaxLength(100);
            address.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
        });

        // GPS coordinates
        builder.Property(e => e.GPSLatitude)
            .HasColumnType("decimal(9,6)");
        builder.Property(e => e.GPSLongitude)
            .HasColumnType("decimal(9,6)");

        // Other properties
        builder.Property(e => e.IsActive)
            .IsRequired();
        builder.Property(e => e.CreatedAtUtc)
            .IsRequired();
        builder.Property(e => e.UpdatedAtdUtc)
            .IsRequired(false); // Make nullable if it can be null

        // Collections configuration - FIXED (use proper navigation properties)
        builder.HasMany(e => e.Departments)
            .WithOne(e => e.HealthcareFacility)
            .HasForeignKey(d => d.FacilityId) // Use shadow property if needed
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Schedules)
            .WithOne(e => e.HealthCareFacility)
            .HasForeignKey(d => d.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.ScheduleExceptions)
            .WithOne(d => d.HealthCareFacility)
            .HasForeignKey(d => d.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Doctors)
            .WithOne(d => d.HealthcareFacility)
            .HasForeignKey(d => d.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(e => e.User) // Use navigation property
            .WithMany(u => u.HealthCareFacilities)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Query filter
        builder.HasQueryFilter(e => e.IsActive);
    }
}
