using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Contracts.Requests.Phones
{
    public record AddPhoneRequest(
        string PhoneNumber,
        string Label,
        bool IsPrimary = false
    );
}