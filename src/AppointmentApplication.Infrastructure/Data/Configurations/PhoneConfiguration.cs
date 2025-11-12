using AppointmentApplication.Domain.Phones;
using AppointmentApplication.Domain.Shared;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.ToTable("phones");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        builder.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
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
            .WithMany(u => u.Phones)
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // ✅ فهرس خاص بالإيميل الأساسي لكل مستخدم
        builder.HasIndex(e => new { e.UserId, e.IsPrimary })
            .HasFilter("[IsPrimary] = 1")
            .IsUnique();

        // ✅ فهرس إضافي للبحث السريع حسب المستخدم أو البريد
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.PhoneNumber);


    }
}

