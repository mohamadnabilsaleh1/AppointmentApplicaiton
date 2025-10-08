using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.Schedules.Queries
{
    public sealed record GetSchedulesByIdQuery(
    Guid HealthCareFacilityId) : IRequest<Result<List<ScheduleDto>>>;
}