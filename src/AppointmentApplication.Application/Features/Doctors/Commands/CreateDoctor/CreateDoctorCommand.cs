using System;

using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Doctors.Enums;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.Commands.CreateDoctor
{
    public sealed record CreateDoctorCommand(
        Guid UserId,
        string FirstName,
        string LastName,
        string Email,
        string Password,
        Gender Gender,
        string LicenseNumber,
        Specialization Specialization,
        DateOnly DateOfBirth
    ) : IRequest<Result<DoctorDto>>;
}
