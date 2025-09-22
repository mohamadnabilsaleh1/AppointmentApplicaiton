using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;

namespace AppointmentApplication.Domain.Billings.BillingPayments;

public class BillingPayment:AuditableEntity
{
    private BillingPayment() { }

    public Guid BillingID { get; private set; }
    public string PaymentMethod { get; private set; }
    public decimal PaidAmount { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public string TransactionReference { get; private set; }
    public string PaymentStatus { get; private set; }

    public Billing Billing { get; private set; }

    public static BillingPayment Create(Guid billingId, string paymentMethod, decimal paidAmount,
        string transactionReference, string paymentStatus = "Completed")
    {
        return new BillingPayment
        {
            BillingID = billingId,
            PaymentMethod = paymentMethod,
            PaidAmount = paidAmount,
            PaymentDate = DateTime.UtcNow,
            TransactionReference = transactionReference,
            PaymentStatus = paymentStatus,
        };
    }

    public void UpdateStatus(string paymentStatus)
    {
        PaymentStatus = paymentStatus;
    }
}
