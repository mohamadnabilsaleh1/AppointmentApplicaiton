using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Dtos;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Queries.GetDoctorTreatmentCapabilityById;

public sealed record GetDoctorTreatmentCapabilityByIdQuery(Guid UserId) : IRequest<Result<DoctorTreatmentCapabilityDto>>;

