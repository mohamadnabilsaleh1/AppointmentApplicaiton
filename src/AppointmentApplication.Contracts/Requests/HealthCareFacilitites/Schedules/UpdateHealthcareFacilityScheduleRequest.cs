using System;
using System.ComponentModel.DataAnnotations;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Contracts.Requests.HealthCareFacilitites
{
    public class UpdateHealthcareFacilityScheduleRequest
    {
        [Required(ErrorMessage = "Day of week is required")]
        [EnumDataType(typeof(DaysOfWeek), ErrorMessage = "Invalid day of week")]
        public DaysOfWeek DayOfWeek { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [EnumDataType(typeof(Status), ErrorMessage = "Invalid status")]
        public Status Status { get; set; }

        public bool IsAvailable { get; set; } = true;

        [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters")]
        public string? Note { get; set; }
    }
}