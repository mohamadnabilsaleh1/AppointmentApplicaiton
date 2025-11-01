using System;
using System.Collections.Generic;
using System.Dynamic;
using AppointmentApplication.Application.Features.Appointments.Dtos;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Services;
using AppointmentApplication.Domain.Appointments.Enums;
using AppointmentApplication.Domain.Shared.Results;
using MediatR;

namespace AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentByDoctorId
{
    public sealed record GetAppointmentsForDoctorCommand(
        Guid UserId,
        DateOnly? StartDate = null,
        DateOnly? EndDate = null,
        AppointmentStatus? Status = null,
        string? Search = null,
        string? Sort = null,
        int Page = 1,
        int PageSize = 10,
        string? Fields = null) : ICachedQuery<Result<PaginationResult<ExpandoObject>>>
    {
        public string CacheKey => $"appointments-for-doctor-{UserId}-{StartDate}-{EndDate}-{Status}-{Search}-{Page}-{PageSize}";
        public string[] Tags => new[] { "appointments", $"doctor-{UserId}" };
        public TimeSpan Expiration => TimeSpan.FromMinutes(5);
    }
}