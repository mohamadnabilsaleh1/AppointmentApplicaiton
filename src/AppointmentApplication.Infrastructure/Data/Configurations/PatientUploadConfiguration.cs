// ReviewsUploadsConfiguration.cs
using AppointmentApplication.Domain.MediaUploads;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class PatientUploadConfiguration : IEntityTypeConfiguration<PatientUpload>
    {
        public void Configure(EntityTypeBuilder<PatientUpload> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.FileType).IsRequired().HasMaxLength(20).HasDefaultValue("Image");
            builder.Property(e => e.FileURL).IsRequired();
            builder.Property(e => e.Title).HasMaxLength(100);
            builder.Property(e => e.Description);
            builder.Property(e => e.Visibility).IsRequired().HasMaxLength(20).HasDefaultValue("Public");
            builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(e => e.UploadedAt).IsRequired();
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.UpdatedAtdUtc);

            builder.HasOne(pu => pu.Patient)
                .WithMany()
                .HasForeignKey(pu => pu.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(e => e.IsActive);
        }
    }
