using AppointmentApplication.Domain.Prescriptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class PrescriptionConfiguration : IEntityTypeConfiguration<Prescription>
{
    public void Configure(EntityTypeBuilder<Prescription> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.DateIssued).IsRequired();
        builder.Property(e => e.MedicationList).IsRequired();
        builder.Property(e => e.DosageInstructions).IsRequired();
        builder.Property(e => e.Status).IsRequired().HasMaxLength(20);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);

        builder.HasOne(p => p.Appointment)
            .WithMany(a => a.Prescriptions)
            .HasForeignKey(p => p.AppointmentID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Doctor)
            .WithMany()
            .HasForeignKey(p => p.IssuedByDoctorID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
