using System;
using System.Dynamic;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Doctors.Enums;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetTopDoctors
{
    public sealed record GetTopDoctorsQuery(
        string? Search,
        int Page = 1,
        int PageSize = 10,
        string? Sort = null,
        string? Fields = null,
        Specialization? Specialization = null) : IRequest<Result<PaginationResult<ExpandoObject>>>, ICachedQuery<Result<PaginationResult<ExpandoObject>>>
    {
        public string CacheKey => "top-doctors";
        public string[] Tags => new[] { "doctors", "top-doctors" };
        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}
