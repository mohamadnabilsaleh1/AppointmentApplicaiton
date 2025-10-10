using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AppointmentApplication.Domain.Patients.Allergies;
using AppointmentApplication.Domain.Patients.Allergies.Enums;

namespace AppointmentApplication.Infrastructure.Data.Configurations
{
    public class AllergyConfiguration : IEntityTypeConfiguration<Allergy>
    {
        public void Configure(EntityTypeBuilder<Allergy> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
            builder.HasMany(c => c.Patients)
                .WithMany(patient => patient.Allergies);
            builder.HasData(Allergy.GetAll());

            // ✅ Use stable IDs (based on enum integer value)
        }
    }
}
