using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Features.Phones.Dtos
{
    public sealed record PhoneDto(
        Guid Id,
        string PhoneNumber,
        string Label,
        bool IsPrimary
    );
}