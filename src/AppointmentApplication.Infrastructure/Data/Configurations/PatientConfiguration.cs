using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.NationalID).IsRequired().HasMaxLength(20);

            builder.Property(e => e.Gender).IsRequired().HasMaxLength(10);
            builder.Property(e => e.DateOfBirth).IsRequired();
            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.UpdatedAtdUtc);

            builder.HasIndex(e => e.NationalID).IsUnique();
            builder.HasQueryFilter(e => e.IsActive);

            // ✅ Explicitly map User navigation to User.Patients collection
            builder.HasOne(e => e.User)
                   .WithMany(u => u.Patients)
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
