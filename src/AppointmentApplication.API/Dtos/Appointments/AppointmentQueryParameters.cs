using System;
using System.ComponentModel.DataAnnotations;
using AppointmentApplication.Domain.Appointments.Enums;

using Microsoft.AspNetCore.Mvc;

namespace AppointmentApplication.API.Models.Appointments
{
    public record AppointmentQueryParameters
    {
        [FromQuery]
        public DateOnly? StartDate { get; init; }

        [FromQuery]
        public DateOnly? EndDate { get; init; }

        [FromQuery]
        public AppointmentStatus? Status { get; init; }

        [FromQuery]
        public string? Search { get; init; }

        [FromQuery]
        public string? Sort { get; init; } = "ScheduledDate";

        [FromQuery]
        [Range(1, int.MaxValue)]
        public int Page { get; init; } = 1;

        [FromQuery]
        [Range(1, 100)]
        public int PageSize { get; init; } = 10;

        [FromQuery]
        public string? Fields { get; init; }
    }
}