using AppointmentApplication.Domain.Patients.ChronicDiseases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class ChronicDiseaseConfiguration : IEntityTypeConfiguration<ChronicDisease>
{
    public void Configure(EntityTypeBuilder<ChronicDisease> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
    }
}
