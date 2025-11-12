using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Contracts.Requests.Phones
{
    public record UpdatePhoneRequest(
        string? PhoneNumber = null,
        string? Label = null,
        bool? IsPrimary = null
    );
}