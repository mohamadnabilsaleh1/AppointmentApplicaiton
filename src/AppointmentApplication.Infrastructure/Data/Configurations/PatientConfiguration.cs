using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Users;
using AppointmentApplication.Domain.Patients.Allergies;
using AppointmentApplication.Domain.Patients.ChronicDiseases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.NationalID).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Gender).IsRequired().HasMaxLength(10);
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            builder.Property(e => e.DateOfBirth).IsRequired();
            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.UpdatedAtdUtc);

            builder.HasIndex(e => e.NationalID).IsUnique();
            builder.HasQueryFilter(e => e.IsActive);

            // ✅ Map Patient → User
            builder.HasOne(e => e.User)
                   .WithMany(u => u.Patients)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // ✅ Many-to-Many: Patient ↔ Allergy
            builder.HasMany(p => p.Allergies)
                   .WithMany(a => a.Patients)
                   .UsingEntity<Dictionary<string, object>>(
                        "PatientAllergies", // join table name
                        j => j.HasOne<Allergy>()
                              .WithMany()
                              .HasForeignKey("AllergyId")
                              .OnDelete(DeleteBehavior.Cascade),
                        j => j.HasOne<Patient>()
                              .WithMany()
                              .HasForeignKey("PatientId")
                              .OnDelete(DeleteBehavior.Cascade),
                        j =>
                        {
                            j.HasKey("PatientId", "AllergyId");
                            j.ToTable("PatientAllergies");
                        });

            // ✅ Many-to-Many: Patient ↔ ChronicDisease
            builder.HasMany(p => p.ChronicDiseases)
                   .WithMany(c => c.Patients)
                   .UsingEntity<Dictionary<string, object>>(
                        "PatientChronicDiseases",
                        j => j.HasOne<ChronicDisease>()
                              .WithMany()
                              .HasForeignKey("ChronicDiseaseId")
                              .OnDelete(DeleteBehavior.Cascade),
                        j => j.HasOne<Patient>()
                              .WithMany()
                              .HasForeignKey("PatientId")
                              .OnDelete(DeleteBehavior.Cascade),
                        j =>
                        {
                            j.HasKey("PatientId", "ChronicDiseaseId");
                            j.ToTable("PatientChronicDiseases");
                        });
        }
    }
}
