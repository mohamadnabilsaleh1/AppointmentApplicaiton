using AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class DoctorTreatmentCapacityConfiguration : IEntityTypeConfiguration<DoctorTreatmentCapacity>
{
    public void Configure(EntityTypeBuilder<DoctorTreatmentCapacity> builder)
    {
        // Primary key
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        // Required properties with default values
        builder.Property(e => e.MaxPatientsPerDay).IsRequired().HasDefaultValue(10);
        builder.Property(e => e.SessionDurationMinutes).IsRequired().HasDefaultValue(30);
        builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);

        // One-to-One relationship with Doctor
        builder.HasOne(dtc => dtc.Doctor)
               .WithOne(d => d.TreatmentCapacity)  // <-- استخدام الـ Navigation property من Doctor
               .HasForeignKey<DoctorTreatmentCapacity>(dtc => dtc.DoctorId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);  // منع حذف الطبيب إذا كان لديه سجل

        // Unique index to enforce one-to-one
        builder.HasIndex(e => e.DoctorId).IsUnique();

        // Query filter for active records
        builder.HasQueryFilter(e => e.IsActive);
    }
}
