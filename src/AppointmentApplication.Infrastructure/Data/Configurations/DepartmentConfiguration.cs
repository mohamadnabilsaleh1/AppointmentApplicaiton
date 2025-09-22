// DepartmentConfiguration.cs
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(255);
            builder.Property(e => e.Description);
            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.UpdatedAtdUtc);

            builder.HasOne<HealthCareFacility>()
                .WithMany(f => f.Departments)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(e => e.IsActive);
        }
    }
