using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Citizens;
using AppointmentApplication.Domain.Patients;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Users.RegisterPatient
{
    public sealed record RegisterPatientCommand(string PhoneNumber, long NationalId, string Email, string Password) : IRequest<Result<Guid>>;
}
