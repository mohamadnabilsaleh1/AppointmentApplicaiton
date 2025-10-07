using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Services;

using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Doctors.Enums;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetDoctors
{
    public sealed record GetDoctorsQuery(
        string? Search,
        int Page = 1,
        int PageSize = 10,
        string? Sort = null,
        string? Fields = null,
        Specialization? Specialization = null) : ICachedQuery<Result<PaginationResult<ExpandoObject>>>
    {
        public string CacheKey => "doctors";
        public string[] Tags => new[] { "doctors" };
        public TimeSpan Expiration => TimeSpan.FromMinutes(10);

    }
}
