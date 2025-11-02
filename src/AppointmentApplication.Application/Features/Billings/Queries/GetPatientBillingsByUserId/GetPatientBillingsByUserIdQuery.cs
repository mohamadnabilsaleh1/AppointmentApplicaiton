using System;
using System.Collections.Generic;
using System.Dynamic;
using AppointmentApplication.Application.Features.Billings.Dtos;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Billings.Queries.GetPatientBillingsByUserId
{
    public sealed record GetPatientBillingsByUserIdQuery(
        Guid UserId,
        DateTime? StartDate = null,
        DateTime? EndDate = null,
        string? Status = null,
        string? Search = null,
        string? Sort = null,
        int Page = 1,
        int PageSize = 10,
        string? Fields = null) : ICachedQuery<Result<PaginationResult<ExpandoObject>>>
    {
        public string CacheKey => $"patient-billings:{UserId}:{StartDate}:{EndDate}:{Status}:{Page}:{PageSize}";
        public string[] Tags => new[] { "billings", $"patient:{UserId}" };
        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }
}