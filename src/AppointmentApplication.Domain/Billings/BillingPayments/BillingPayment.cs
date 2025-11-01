using System;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Billings;

namespace AppointmentApplication.Domain.Billings.BillingPayments;

public class BillingPayment : AuditableEntity
{
    private BillingPayment() { }

    public Guid BillingID { get; private set; }
    public string PaymentMethod { get; private set; }
    public decimal PaidAmount { get; private set; }
    public DateTime PaymentDate { get; private set; }

    // 🔗 One-to-one navigation
    public Billing Billing { get; private set; }

    // 🏗 Factory Method
    public static BillingPayment Create(Guid billingId, string paymentMethod, decimal paidAmount,
        string transactionReference)
    {
        return new BillingPayment
        {
            BillingID = billingId,
            PaymentMethod = paymentMethod,
            PaidAmount = paidAmount,
            PaymentDate = DateTime.UtcNow,
        };
    }

}
