using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Contracts.Requests.Emails
{
    public record UpdateEmailRequest(
        string? EmailAddress = null,
        string? Label = null,
        bool? IsPrimary = null
    );
}