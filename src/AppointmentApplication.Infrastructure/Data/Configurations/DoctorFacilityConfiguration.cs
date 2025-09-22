// DoctorFacilityConfiguration.cs
using AppointmentApplication.Domain.DoctorFacilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class DoctorFacilityConfiguration : IEntityTypeConfiguration<DoctorFacility>
    {
        public void Configure(EntityTypeBuilder<DoctorFacility> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.UpdatedAtdUtc);

            builder.HasOne(df => df.Doctor)
                .WithMany(d => d.Facilities)
                .HasForeignKey(df => df.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(df => df.Facility)
                .WithMany()
                .HasForeignKey(df => df.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => new { e.DoctorId, e.FacilityId }).IsUnique();
            builder.HasQueryFilter(e => e.IsActive);
        }
    }
