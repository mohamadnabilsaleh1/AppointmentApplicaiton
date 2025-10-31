using AppointmentApplication.Domain.Billings;
using AppointmentApplication.Domain.Billings.BillingPayments;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class BillingConfiguration : IEntityTypeConfiguration<Billing>
{
    public void Configure(EntityTypeBuilder<Billing> builder)
    {
        builder.ToTable("billings");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.DateIssued).IsRequired();
        builder.Property(e => e.TotalAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.Status)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Notes).HasMaxLength(500);

        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtUtc); // ✅ fixed typo (was UpdatedAtdUtc)

        // ✅ One-to-One: Billing ↔ Appointment
        builder.HasOne(b => b.Appointment)
            .WithOne(a => a.Billing)
            .HasForeignKey<Billing>(b => b.AppointmentID)
            .OnDelete(DeleteBehavior.Restrict);

        // ✅ Optional One-to-One: Billing ↔ BillingPayment
        builder.HasOne(b => b.BillingPayment)
            .WithOne(p => p.Billing)
            .HasForeignKey<BillingPayment>(p => p.BillingID)
            .OnDelete(DeleteBehavior.Cascade);

        // ✅ Relationships with Doctor & Patient (Many-to-One)
        builder.HasOne(b => b.Doctor)
            .WithMany()
            .HasForeignKey(b => b.DoctorID)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Patient)
            .WithMany()
            .HasForeignKey(b => b.PatientID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
