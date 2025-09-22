// PatientAllergyConfiguration.cs
using AppointmentApplication.Domain.Patients.PatientAllergies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class PatientAllergyConfiguration : IEntityTypeConfiguration<PatientAllergy>
    {
        public void Configure(EntityTypeBuilder<PatientAllergy> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.UpdatedAtdUtc);

            builder.HasOne(pa => pa.Patient)
                .WithMany(p => p.Allergies)
                .HasForeignKey(pa => pa.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.PatientId, e.AllergyId }).IsUnique();
        }
    }
