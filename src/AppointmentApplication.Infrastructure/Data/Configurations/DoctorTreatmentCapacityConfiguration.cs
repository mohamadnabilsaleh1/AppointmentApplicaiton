using AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class DoctorTreatmentCapacityConfiguration : IEntityTypeConfiguration<DoctorTreatmentCapacity>
{
    public void Configure(EntityTypeBuilder<DoctorTreatmentCapacity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.MaxPatientsPerDay).IsRequired().HasDefaultValue(10);
        builder.Property(e => e.SessionDurationMinutes).IsRequired().HasDefaultValue(30);
        builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);

        builder.HasOne(dtc => dtc.Doctor)
            .WithOne()
            .HasForeignKey<DoctorTreatmentCapacity>(dtc => dtc.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => e.DoctorId).IsUnique();
        builder.HasQueryFilter(e => e.IsActive);
    }
}
