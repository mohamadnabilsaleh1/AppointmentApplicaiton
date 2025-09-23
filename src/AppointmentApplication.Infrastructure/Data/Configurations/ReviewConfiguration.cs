using AppointmentApplication.Domain.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

// Reviews & Uploads Configurations
public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Rating).IsRequired();
        builder.Property(e => e.Comment);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);

        builder.HasOne(r => r.Patient)
            .WithMany()
            .HasForeignKey(r => r.PatientID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Facility)
            .WithMany()
            .HasForeignKey(r => r.FacilityID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Doctor)
            .WithMany()
            .HasForeignKey(r => r.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
