using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Doctors.Enums;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Doctors.Queries.GetDoctorsByHealthCareFacilityId
{
    public sealed record GetDoctorsByHealthCareFacilityIdQuery(
        Guid HealthCareFacilityId) : ICachedQuery<Result<List<DoctorDto>>>
    {
        public string CacheKey => "healthCareFacilityDoctors";
        public string[] Tags => new[] { "healthCareFacilityDoctors" };
        public TimeSpan Expiration => TimeSpan.FromMinutes(10);
    }
}

