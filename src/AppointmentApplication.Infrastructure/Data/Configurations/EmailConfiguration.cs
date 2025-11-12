using AppointmentApplication.Domain.Emails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

// Contact Information Configurations
public class EmailConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.ToTable("emails");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.EmailAddress)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Label)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.IsPrimary)
            .IsRequired();

        builder.Property(e => e.CreatedAtUtc)
            .IsRequired();

        builder.Property(e => e.UpdatedAtUtc);

        // ✅ العلاقة مع User (جداً مهمة)
        builder.HasOne(e => e.User)
            .WithMany(u => u.Emails)
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // ✅ فهرس خاص بالإيميل الأساسي لكل مستخدم
        builder.HasIndex(e => new { e.UserId, e.IsPrimary })
            .HasFilter("[IsPrimary] = 1")
            .IsUnique();

        // ✅ فهرس إضافي للبحث السريع حسب المستخدم أو البريد
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.EmailAddress);
    }
}
