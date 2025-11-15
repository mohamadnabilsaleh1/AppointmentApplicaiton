using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Contracts.Requests.Doctors
{
    public record UpdateDescriptionRequest(string Description);
}