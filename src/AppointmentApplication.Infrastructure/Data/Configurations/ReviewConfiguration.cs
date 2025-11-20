using AppointmentApplication.Domain.Reviews;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        
        // Properties
        builder.Property(e => e.Rating)
               .IsRequired()
               .HasAnnotation("MinValue", 1)
               .HasAnnotation("MaxValue", 5);
        
        builder.Property(e => e.Comment)
               .HasMaxLength(1000);
        
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtUtc);

        // ✅ Indexes for better query performance
        builder.HasIndex(r => r.DoctorID);
        builder.HasIndex(r => r.PatientID);
        builder.HasIndex(r => r.AppointmentId).IsUnique(); // One review per appointment
        builder.HasIndex(r => r.Rating);
        builder.HasIndex(r => r.CreatedAtUtc);

        // ✅ Composite index for common queries
        builder.HasIndex(r => new { r.DoctorID, r.CreatedAtUtc });

        // Relationships
        builder.HasOne(r => r.Patient)
            .WithMany()
            .HasForeignKey(r => r.PatientID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Facility)
            .WithMany()
            .HasForeignKey(r => r.FacilityID)
            .OnDelete(DeleteBehavior.Restrict);

        // ✅ Doctor relationship (Many Reviews -> One Doctor)
        builder.HasOne(r => r.Doctor)
            .WithMany(d => d.Reviews)
            .HasForeignKey(r => r.DoctorID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Appointment)
            .WithOne()
            .HasForeignKey<Review>(r => r.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // ✅ Optional: Add check constraint for rating
        builder.ToTable(t => t.HasCheckConstraint("CK_Review_Rating", "[Rating] BETWEEN 1 AND 5"));
    }
}