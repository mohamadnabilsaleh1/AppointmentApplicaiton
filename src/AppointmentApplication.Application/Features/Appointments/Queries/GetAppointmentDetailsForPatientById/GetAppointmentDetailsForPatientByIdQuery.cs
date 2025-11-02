// AppointmentApplication.Application/Features/Appointments/Queries/GetAppointmentDetailsForPatientById/GetAppointmentDetailsForPatientByIdQuery.cs
using AppointmentApplication.Application.Features.Appointments.Dtos;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentDetailsForPatientById
{
    public sealed record GetAppointmentDetailsForPatientByIdQuery(
        Guid UserId,
        Guid AppointmentId,
        string? Fields = null
    ) : IRequest<Result<AppointmentDetailsDto>>;
}