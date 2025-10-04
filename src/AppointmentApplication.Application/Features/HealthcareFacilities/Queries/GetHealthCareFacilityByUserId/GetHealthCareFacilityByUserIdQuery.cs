using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityByUserId
{
    public sealed record GetHealthCareFacilityByUserIdQuery(Guid UserId):IRequest<Result<HealthcareFacilityWithUserDto>>;
}