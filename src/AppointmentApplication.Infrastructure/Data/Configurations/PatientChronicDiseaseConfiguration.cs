// PatientChronicDiseaseConfiguration.cs
using AppointmentApplication.Domain.Patients.PatientChronicDiseases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class PatientChronicDiseaseConfiguration : IEntityTypeConfiguration<PatientChronicDisease>
    {
        public void Configure(EntityTypeBuilder<PatientChronicDisease> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.UpdatedAtdUtc);

            builder.HasOne(pcd => pcd.Patient)
                .WithMany(p => p.ChronicDiseases)
                .HasForeignKey(pcd => pcd.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.PatientId, e.ChronicDiseaseId }).IsUnique();
        }
    }
