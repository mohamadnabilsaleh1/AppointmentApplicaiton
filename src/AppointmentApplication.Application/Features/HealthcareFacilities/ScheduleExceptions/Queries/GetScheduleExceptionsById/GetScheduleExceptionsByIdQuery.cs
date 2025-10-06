using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Queries.GetScheduleExceptionsById
{
    public sealed record GetScheduleExceptionsByIdQuery(
    Guid HealthCareFacilityId,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null) : IRequest<Result<List<ScheduleExceptionDto>>>;
}



