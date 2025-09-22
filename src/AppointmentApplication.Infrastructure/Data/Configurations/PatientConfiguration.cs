// PatientConfiguration.cs
using AppointmentApplication.Domain.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.NationalID).IsRequired().HasMaxLength(20);
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Gender).IsRequired().HasMaxLength(10);
            builder.Property(e => e.DateOfBirth).IsRequired();
            builder.Property(e => e.BloodType).HasMaxLength(5);
            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.UpdatedAtdUtc);

            builder.HasIndex(e => e.NationalID).IsUnique();
            builder.HasQueryFilter(e => e.IsActive);
        }
    }
