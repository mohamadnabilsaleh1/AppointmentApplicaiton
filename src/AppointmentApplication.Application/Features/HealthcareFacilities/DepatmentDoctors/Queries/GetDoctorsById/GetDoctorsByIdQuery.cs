using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;

using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Queries.GetSchedulesByIdQuery
{
    public sealed record GetDoctorsByIdQuery(
    Guid HealthCareFacilityId, Guid DepartmentId ) : IRequest<Result<List<DoctorDto>>>;
}