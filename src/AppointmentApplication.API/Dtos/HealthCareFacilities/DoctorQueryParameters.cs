using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Doctors.Enums;

using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.API.Dtos.HealthCareFacilities
{
    public class DoctorQueryParameters : QueryParameters
    {
        public Specialization? Specialization { get; set; }
    }
}

/*

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
*/