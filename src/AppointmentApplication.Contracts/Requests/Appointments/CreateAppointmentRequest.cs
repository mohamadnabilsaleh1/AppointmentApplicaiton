using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentApplication.Api.Models.Appointments
{
    public class CreateAppointmentRequest
    {
        [Required]
        public Guid PatientId { get; set; }

        [Required]
        public Guid DoctorId { get; set; }

        [Required]
        public Guid FacilityId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly ScheduledDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan ScheduledTime { get; set; }

        [Required]
        [Range(15, 480)]
        public int DurationMinutes { get; set; } = 30;

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Notes { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal? TotalAmount { get; set; }
    }
}