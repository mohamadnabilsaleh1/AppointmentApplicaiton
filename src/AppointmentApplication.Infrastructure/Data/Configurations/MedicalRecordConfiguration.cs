// MedicalRecordConfiguration.cs
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
        builder.Property(e => e.RecordType).IsRequired().HasMaxLength(50);
        builder.Property(e => e.DateCreated).IsRequired();
        builder.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Active");
        builder.Property(e => e.Details).IsRequired();
        builder.Property(e => e.Notes);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);

        builder.HasOne(mr => mr.Patient)
            .WithMany(p => p.MedicalRecords)
            .HasForeignKey(mr => mr.PatientID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(mr => mr.Facility)
            .WithMany()
            .HasForeignKey(mr => mr.FacilityID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(mr => mr.Doctor)
            .WithMany()
            .HasForeignKey(mr => mr.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(mr => mr.Appointment)
            .WithMany()
            .HasForeignKey(mr => mr.AppointmentID)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasQueryFilter(e => e.Status != "Deleted");
    }
}
