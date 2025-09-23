using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Schedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class ScheduleHealthcareFacilityConfiguration : IEntityTypeConfiguration<ScheduleHealthcareFacility>
{
    public void Configure(EntityTypeBuilder<ScheduleHealthcareFacility> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.DayOfWeek).IsRequired();
        builder.Property(e => e.StartTime).IsRequired();
        builder.Property(e => e.EndTime).IsRequired();
        builder.Property(e => e.Status).IsRequired().HasMaxLength(50);
        builder.Property(e => e.IsAvailable).IsRequired();
        builder.Property(e => e.Note).HasMaxLength(500);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);

        builder.HasOne<HealthCareFacility>()
            .WithMany(f => f.Schedules)
            .HasForeignKey(s => s.FacilityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
