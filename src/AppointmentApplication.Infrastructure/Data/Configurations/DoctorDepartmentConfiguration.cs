using AppointmentApplication.Domain.DoctorDepartments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class DoctorDepartmentConfiguration : IEntityTypeConfiguration<DoctorDepartment>
{
    public void Configure(EntityTypeBuilder<DoctorDepartment> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.IsActive).IsRequired();
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);

        builder.HasOne(dd => dd.Doctor)
            .WithMany(d => d.Departments)
            .HasForeignKey(dd => dd.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(dd => dd.Department)
            .WithMany(d => d.DoctorDepartments)
            .HasForeignKey(dd => dd.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(dd => dd.Facility)
            .WithMany()
            .HasForeignKey(dd => dd.FacilityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(e => new { e.DoctorId, e.DepartmentId, e.FacilityId }).IsUnique();
        builder.HasQueryFilter(e => e.IsActive);
    }
}
