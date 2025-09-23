using AppointmentApplication.Domain.Appointments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

// Appointments & Billing Configurations
public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.ScheduledDate).IsRequired();
        builder.Property(e => e.ScheduledTime).IsRequired();
        builder.Property(e => e.DurationMinutes).IsRequired().HasDefaultValue(30);
        builder.Property(e => e.Status).IsRequired().HasMaxLength(20);
        builder.Property(e => e.BookingDate).IsRequired();
        builder.Property(e => e.Notes);
        builder.Property(e => e.CancelledReason);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);

        builder.HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Facility)
            .WithMany()
            .HasForeignKey(a => a.FacilityID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Department)
            .WithMany()
            .HasForeignKey(a => a.DepartmentID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
