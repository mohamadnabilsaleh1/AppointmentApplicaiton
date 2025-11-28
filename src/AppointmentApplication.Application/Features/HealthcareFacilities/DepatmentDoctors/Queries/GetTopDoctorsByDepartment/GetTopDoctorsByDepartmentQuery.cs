using System;
using System.Collections.Generic;

using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Queries.GetSchedulesByIdQuery
{
    public sealed record GetTopDoctorsByDepartmentQuery(
        Guid HealthCareFacilityId,
        Guid DepartmentId,
        int Limit = 5) : IRequest<Result<List<DepartmentDoctorsDto>>>;
}
