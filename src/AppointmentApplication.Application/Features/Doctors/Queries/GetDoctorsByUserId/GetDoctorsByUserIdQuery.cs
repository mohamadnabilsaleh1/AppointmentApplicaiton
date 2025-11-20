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
        Guid UserId) : ICachedQuery<Result<List<DoctorWithContactDto>>>
    {
        public string CacheKey => $"healthCareFacilityDoctorsByUser:{UserId}"; // ✅ Include user ID in cache key
        public string[] Tags => new[] { "healthCareFacilityDoctors", $"user:{UserId}" };
        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}