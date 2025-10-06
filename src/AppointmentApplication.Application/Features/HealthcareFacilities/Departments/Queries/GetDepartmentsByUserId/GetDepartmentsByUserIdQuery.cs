using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Dtos;
using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Queries.GetDepartments
{
    public sealed record GetDepartmentsByUserIdQuery(Guid UserId) : ICachedQuery<Result<List<DepartmentDto>>>
    {
        public string CacheKey => "departments";
        public string[] Tags => new[] { "departments" };
        public TimeSpan Expiration => TimeSpan.FromDays(1);
    }

}