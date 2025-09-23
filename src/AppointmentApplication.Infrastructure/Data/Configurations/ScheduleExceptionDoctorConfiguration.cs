using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Doctors.ScheduleExceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class ScheduleExceptionDoctorConfiguration : IEntityTypeConfiguration<ScheduleExceptionDoctor>
{
    public void Configure(EntityTypeBuilder<ScheduleExceptionDoctor> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Date).IsRequired();
        builder.Property(e => e.DayOfWeek).IsRequired();
        builder.Property(e => e.StartTime).IsRequired();
        builder.Property(e => e.EndTime).IsRequired();
        builder.Property(e => e.Status).IsRequired().HasMaxLength(50);
        builder.Property(e => e.Reason).HasMaxLength(500);
        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtdUtc);

        builder.HasOne<Doctor>()
            .WithMany(d => d.ScheduleExceptions)
            .HasForeignKey(s => s.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
