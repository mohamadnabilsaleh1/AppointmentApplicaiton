using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;
using AppointmentApplication.Domain.HealthcareFacilities.Departments;
using AppointmentApplication.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Gender).IsRequired().HasMaxLength(10);
            builder.Property(e => e.DateOfBirth).IsRequired();
            builder.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.Specialization).IsRequired();
            builder.Property(e => e.UpdatedAtdUtc);

            builder.HasQueryFilter(e => e.IsActive);

            // ✅ Explicitly map User navigation to User.Doctors collection
            builder.HasOne(d => d.User)
                   .WithMany(u => u.Doctors)
                   .HasForeignKey(d => d.UserId)
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.HealthcareFacility)
       .WithMany(f => f.Doctors)
       .HasForeignKey(d => d.FacilityId)
       .OnDelete(DeleteBehavior.Cascade);
            // One-to-one relationship with TreatmentCapacity
            builder.HasOne(d => d.TreatmentCapacity)
                   .WithOne(tc => tc.Doctor)
                   .HasForeignKey<DoctorTreatmentCapacity>(tc => tc.DoctorId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);

            // Configure many-to-many relationship
            builder.HasMany(d => d.Departments)
                   .WithMany(d => d.Doctors)
                   .UsingEntity<Dictionary<string, object>>(
                       "DoctorDepartment",
                       j => j.HasOne<Department>().WithMany().HasForeignKey("DepartmentId").OnDelete(DeleteBehavior.Restrict),
                       j => j.HasOne<Doctor>().WithMany().HasForeignKey("DoctorId").OnDelete(DeleteBehavior.Restrict),
                       j =>
                       {
                           j.Property<DateTime>("CreatedAtUtc").IsRequired();
                           j.Property<DateTime?>("UpdatedAtdUtc");
                           j.HasKey("DoctorId", "DepartmentId");
                       });

        }
    }
}
