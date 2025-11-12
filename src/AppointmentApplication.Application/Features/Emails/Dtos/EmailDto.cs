using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Features.Emails.Dtos
{
    public sealed record EmailDto(
        Guid Id,
        string EmailAddress,
        string Label,
        bool IsPrimary
    );
}