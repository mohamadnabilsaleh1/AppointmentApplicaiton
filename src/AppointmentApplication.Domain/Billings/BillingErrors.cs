// Domain/Billings/Errors/BillingErrors.cs
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Billings.Errors
{
    public static class BillingErrors
    {
        public static readonly Error NotFound =
            Error.NotFound("Billing.NotFound", "Billing record not found.");

        public static readonly Error InvalidAppointmentId =
            Error.Validation("Billing.InvalidAppointmentId", "Appointment ID cannot be empty.");

        public static readonly Error InvalidPatientId =
            Error.Validation("Billing.InvalidPatientId", "Patient ID cannot be empty.");

        public static readonly Error InvalidDoctorId =
            Error.Validation("Billing.InvalidDoctorId", "Doctor ID cannot be empty.");

        public static readonly Error InvalidAmount =
            Error.Validation("Billing.InvalidAmount", "Total amount must be greater than zero.");

        public static readonly Error InvalidPaymentAmount =
            Error.Validation(
                "Billing.InvalidPaymentAmount",
                "Payment amount must be greater than zero and cannot exceed total amount.");

        public static readonly Error CurrencyMismatch =
            Error.Validation(
                "Billing.CurrencyMismatch",
                "Payment currency must match billing currency.");

        public static readonly Error InvalidPaymentMethod =
            Error.Validation("Billing.InvalidPaymentMethod", "Invalid payment method.");

        public static readonly Error EmptyTransactionReference =
            Error.Validation(
                "Billing.EmptyTransactionReference",
                "Transaction reference is required.");

        public static readonly Error CannotProcessPaidBilling =
            Error.Conflict(
                "Billing.CannotProcessPaid",
                "Cannot process payment for already paid billing.");

        public static readonly Error CannotRefundUnpaidBilling =
            Error.Conflict(
                "Billing.CannotRefundUnpaid",
                "Cannot refund billing that hasn't been paid.");

        public static readonly Error CannotUpdatePaidBilling =
            Error.Conflict(
                "Billing.CannotUpdatePaid",
                "Cannot update amount for paid billing.");

        public static readonly Error EmptyRefundReason =
            Error.Validation("Billing.EmptyRefundReason", "Refund reason is required.");

        public static readonly Error DueDateBeforeIssueDate =
            Error.Validation(
                "Billing.DueDateBeforeIssueDate",
                "Due date must be after issue date.");
    }
}