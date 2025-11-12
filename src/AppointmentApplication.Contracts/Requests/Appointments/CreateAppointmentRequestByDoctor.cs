using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Contracts.Requests.Appointments
{
    public class CreateAppointmentRequestByDoctor
    {
        [Required]
        public Guid PatientId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly ScheduledDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan ScheduledTime { get; set; }

        [Required]
        [Range(15, 480)]
        public int DurationMinutes { get; set; } = 30;

        [Range(0.01, double.MaxValue)]
        public decimal? TotalAmount { get; set; }

        // ✅ إضافة حقل Notes
        public string? Notes { get; set; }
    }
}