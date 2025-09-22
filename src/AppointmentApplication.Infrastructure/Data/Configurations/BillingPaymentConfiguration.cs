// AppointmentBillingConfiguration.cs
using AppointmentApplication.Domain.Billings.BillingPayments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppointmentApplication.Infrastructure.Data.Configurations;

public class BillingPaymentConfiguration : IEntityTypeConfiguration<BillingPayment>
    {
        public void Configure(EntityTypeBuilder<BillingPayment> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(20);
            builder.Property(e => e.PaidAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(e => e.PaymentDate).IsRequired();
            builder.Property(e => e.TransactionReference);
            builder.Property(e => e.PaymentStatus).IsRequired().HasMaxLength(20).HasDefaultValue("Completed");
            builder.Property(e => e.CreatedAtUtc).IsRequired();
            builder.Property(e => e.UpdatedAtdUtc);

            builder.HasOne(bp => bp.Billing)
                .WithMany(b => b.Payments)
                .HasForeignKey(bp => bp.BillingID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
