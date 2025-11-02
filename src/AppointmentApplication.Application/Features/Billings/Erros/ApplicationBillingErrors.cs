using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Application.Features.Billings.Erros
{
    public class ApplicationBillingErrors
    {

        public static Error InvalidStatusFilter(string status) =>
            Error.Validation(
                "Billing.Query.InvalidStatus",
                $"Invalid billing status filter: '{status}'. Valid values are: Pending, Paid, Overdue, Refunded, Cancelled");

        public static readonly Error NoBillingsFound =
            Error.NotFound(
                "Billing.Query.NoBillings",
                "No billings found for the specified criteria.");

        public static readonly Error InvalidDateRange =
            Error.Validation(
                "Billing.Query.InvalidDateRange",
                "End date must be greater than or equal to start date.");
    }
}
