using AppointmentApplication.Domain.MedicalRecords;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtUtc);

        // Explicitly configure AppointmentId as required
        builder.Property(mr => mr.AppointmentId)
            .IsRequired();

        builder.HasOne(mr => mr.Patient)
            .WithMany(p => p.MedicalRecords)
            .HasForeignKey(mr => mr.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(mr => mr.Facility)
            .WithMany()
            .HasForeignKey(mr => mr.FacilityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(mr => mr.Doctor)
            .WithMany()
            .HasForeignKey(mr => mr.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(mr => mr.Appointment)
            .WithMany()
            .HasForeignKey(mr => mr.AppointmentId)
            .IsRequired() // Ensure it's required
            .OnDelete(DeleteBehavior.Restrict); // Change from SetNull to Restrict
    }
}