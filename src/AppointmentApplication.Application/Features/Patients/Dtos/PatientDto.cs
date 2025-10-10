using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Enums;

using Microsoft.Identity.Client;

namespace AppointmentApplication.Application.Features.Patients.Dtos
{
    public sealed record PatientDto(Guid Id, string NationalID, Gender Gender ,string FirstName, string LastName, DateOnly DateOfBirth);
}

