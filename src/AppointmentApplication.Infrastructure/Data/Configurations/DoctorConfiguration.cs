using AppointmentApplication.Domain.Doctors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

// Doctor Entities Configurations
public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Gender).IsRequired().HasMaxLength(10);
        builder.Property(e => e.DateOfBirth).IsRequired();
        builder.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);

        builder.HasOne(d => d.Specialization)
            .WithMany(s => s.Doctors)
            .HasForeignKey(d => d.SpecializationID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(e => e.IsActive);
    }
}
