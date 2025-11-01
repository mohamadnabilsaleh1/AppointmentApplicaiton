using System.ComponentModel.DataAnnotations;

namespace AppointmentApplication.Api.Models.Appointments
{
    public record CancelAppointmentRequest
    {
        [Required(ErrorMessage = "Cancellation reason is required.")]
        [StringLength(500, ErrorMessage = "Cancellation reason cannot exceed 500 characters.")]
        public string CancellationReason { get; init; } = string.Empty;
    }
}