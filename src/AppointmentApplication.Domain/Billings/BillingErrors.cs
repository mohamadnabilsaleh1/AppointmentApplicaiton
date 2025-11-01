using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Billings
{
    public static class BillingErrors
    {
        public static readonly Error BillingNotFound =
            Error.NotFound("Billing.NotFound", "Billing not found.");

        public static readonly Error InvalidTotalAmount =
            Error.Validation("Billing.InvalidTotalAmount", "Total amount must be greater than 0.");

        public static readonly Error InvalidPaidAmount =
            Error.Validation("Billing.InvalidPaidAmount", "Paid amount must be greater than 0.");

        public static readonly Error BillingAlreadyPaid =
            Error.Conflict("Billing.AlreadyPaid", "Billing is already paid.");

        public static readonly Error PartialPaymentNotAllowed =
            Error.Conflict("Billing.PartialPaymentNotAllowed", "Partial payments are not allowed. Full amount must be paid.");

        public static readonly Error CannotCancelPaidBilling =
            Error.Conflict("Billing.CannotCancelPaid", "Cannot cancel paid billing.");
    }
}