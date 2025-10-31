using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Commands.UpdateDoctorsTreatmentCapability;

public sealed record UpdateDoctorsTreatmentCapabilityCommand(Guid UserId, int MaxPatientsPerDay,
int SessionDurationMinutes): IRequest<Result<Updated>>;