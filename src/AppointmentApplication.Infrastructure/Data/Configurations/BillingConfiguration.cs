// AppointmentBillingConfiguration.cs
using AppointmentApplication.Domain.Billings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class BillingConfiguration : IEntityTypeConfiguration<Billing>
    {
        public void Configure(EntityTypeBuilder<Billing> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.DateIssued).IsRequired();
            builder.Property(e => e.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Pending");
            builder.Property(e => e.Notes);
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.UpdatedAtdUtc);

            builder.HasOne(b => b.Patient)
                .WithMany()
                .HasForeignKey(b => b.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Facility)
                .WithMany()
                .HasForeignKey(b => b.FacilityID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Appointment)
                .WithMany(a => a.Billings)
                .HasForeignKey(b => b.AppointmentID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Doctor)
                .WithMany()
                .HasForeignKey(b => b.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
