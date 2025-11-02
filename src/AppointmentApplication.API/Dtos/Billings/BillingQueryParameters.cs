using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace AppointmentApplication.API.Models.Billings
{
    public record BillingQueryParameters
    {
        [FromQuery]
        public DateTime? StartDate { get; init; }

        [FromQuery]
        public DateTime? EndDate { get; init; }

        [FromQuery]
        public string? Status { get; init; }

        [FromQuery]
        public string? Search { get; init; }

        [FromQuery]
        public string? Sort { get; init; } = "DateIssued desc";

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