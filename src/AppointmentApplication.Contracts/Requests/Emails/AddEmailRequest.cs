using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Contracts.Requests.Emails
{
    public record AddEmailRequest(
        string EmailAddress,
        string Label,
        bool IsPrimary = false
    );
}