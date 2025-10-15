using AppointmentApplication.Domain.Billings.BillingPayments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class BillingPaymentConfiguration : IEntityTypeConfiguration<BillingPayment>
{
    public void Configure(EntityTypeBuilder<BillingPayment> builder)
    {
        builder.ToTable("billing_payments");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.PaidAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.PaymentDate).IsRequired();
        builder.Property(e => e.TransactionReference).HasMaxLength(100);

        builder.Property(e => e.PaymentStatus)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("Completed");

        builder.Property(e => e.CreatedAtUtc).IsRequired();
        builder.Property(e => e.UpdatedAtUtc); // ✅ fixed typo (was UpdatedAtdUtc)

        // ✅ One-to-One: Billing ↔ BillingPayment
        builder.HasOne(bp => bp.Billing)
            .WithOne(b => b.BillingPayment)
            .HasForeignKey<BillingPayment>(bp => bp.BillingID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
