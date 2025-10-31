using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Dtos;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.DoctorsTreatmentCapabilities.Queries.GetDoctorTreatmentCapabilityByDoctorId;

public sealed record GetDoctorTreatmentCapabilityByDoctorIdQuery(Guid DoctorId) : IRequest<Result<DoctorTreatmentCapabilityDto>>;

