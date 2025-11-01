using AppointmentApplication.Domain.Billings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations
{
    public class BillingConfiguration : IEntityTypeConfiguration<Billing>
    {
        public void Configure(EntityTypeBuilder<Billing> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            // Configure properties
            builder.Property(e => e.TotalAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(e => e.PaidAmount)
                .HasPrecision(18, 2); // Add this

            builder.Property(e => e.DateIssued)
                .IsRequired();

            builder.Property(e => e.PaymentDate); // Add this

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            // Configure relationships
            builder.HasOne(b => b.Appointment)
                .WithOne(a => a.Billing)
                .HasForeignKey<Billing>(b => b.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Patient)
                .WithMany()
                .HasForeignKey(b => b.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Doctor)
                .WithMany()
                .HasForeignKey(b => b.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}