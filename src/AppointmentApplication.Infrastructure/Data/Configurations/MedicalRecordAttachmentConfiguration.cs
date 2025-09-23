using AppointmentApplication.Domain.MedicalRecordAttachments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class MedicalRecordAttachmentConfiguration : IEntityTypeConfiguration<MedicalRecordAttachment>
{
    public void Configure(EntityTypeBuilder<MedicalRecordAttachment> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.FileType).IsRequired().HasMaxLength(20).HasDefaultValue("Image");
        builder.Property(e => e.FileURL).IsRequired();
        builder.Property(e => e.Title).HasMaxLength(100);
        builder.Property(e => e.Description);
        builder.Property(e => e.Visibility).IsRequired().HasMaxLength(20).HasDefaultValue("Private");
        builder.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(e => e.UploadedAt).IsRequired();
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);

        builder.HasOne(mra => mra.MedicalRecord)
            .WithMany(mr => mr.Attachments)
            .HasForeignKey(mra => mra.MedicalRecordID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(e => e.IsActive);
    }
}
