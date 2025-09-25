using System;
using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityById
{
    public sealed record GetHealthCareFacilityByIdQuery(Guid Id)
        : IRequest<Result<HealthcareFacilityDto>>;
}
