using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Commands.CreateDoctorsTreatmentCapability;

public sealed record DeleteDoctorsTreatmentCapabilityCommand(Guid UserId): IRequest<Result<Deleted>>;