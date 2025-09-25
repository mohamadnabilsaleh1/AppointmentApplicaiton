using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Application.Shared.Interfaces;
using MediatR;
using AppointmentApplication.Application.Shared.Services;
using System;
using System.Dynamic;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilities
{
    public sealed record GetHealthCareFacilityQuery(
        string? Search,
        int Page = 1,
        int PageSize = 10,
        string? Sort = null,
        string? Fields = null,
        HealthCareType? Type = null,
        string? Street = null,
        string? City = null,
        string? State = null,
        string? Country = null,
        string? ZipCode = null
    ) : ICachedQuery<Result<PaginationResult<ExpandoObject>>>,
        IRequest<Result<PaginationResult<ExpandoObject>>>
    {
        public string CacheKey => "healthCareFacilities";
        public string[] Tags => new[] { "healthCareFacility" };
        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
