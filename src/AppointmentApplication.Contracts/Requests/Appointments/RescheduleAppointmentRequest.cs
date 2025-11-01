using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentApplication.Api.Models.Appointments
{
    public record RescheduleAppointmentRequest
    {
        [Required(ErrorMessage = "New date is required.")]
        public DateOnly NewDate { get; init; }

        [Required(ErrorMessage = "New time is required.")]
        public TimeSpan NewTime { get; init; }

        [StringLength(500, ErrorMessage = "Reschedule reason cannot exceed 500 characters.")]
        public string? Reason { get; init; }
    }
}