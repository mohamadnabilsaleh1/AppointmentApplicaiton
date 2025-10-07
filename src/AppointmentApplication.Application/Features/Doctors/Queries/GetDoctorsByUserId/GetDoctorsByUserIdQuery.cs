using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;

using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorByUserId
{
    public sealed record GetDoctorsByUserIdQuery(
        Guid UserId) : ICachedQuery<Result<List<DoctorDto>>>
    {
        public string CacheKey => "healthCareFacilityDoctors";
        public string[] Tags => new[] { "healthCareFacilityDoctors" };
        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}