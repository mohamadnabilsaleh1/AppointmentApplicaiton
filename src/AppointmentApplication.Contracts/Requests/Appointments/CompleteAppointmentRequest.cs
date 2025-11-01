using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppointmentApplication.Api.Models.Appointments
{
    public record CompleteAppointmentRequest
    {
        [Required(ErrorMessage = "Diagnosis is required.")]
        [StringLength(500, ErrorMessage = "Diagnosis cannot exceed 500 characters.")]
        public string Diagnosis { get; init; } = string.Empty;

        [Required(ErrorMessage = "Treatment notes are required.")]
        [StringLength(2000, ErrorMessage = "Treatment notes cannot exceed 2000 characters.")]
        public string TreatmentNotes { get; init; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Follow-up instructions cannot exceed 1000 characters.")]
        public string FollowUpInstructions { get; init; } = string.Empty;

        [Required(ErrorMessage = "Medication list is required.")]
        [StringLength(1000, ErrorMessage = "Medication list cannot exceed 1000 characters.")]
        public string MedicationList { get; init; } = string.Empty;

        [Required(ErrorMessage = "Dosage instructions are required.")]
        [StringLength(1000, ErrorMessage = "Dosage instructions cannot exceed 1000 characters.")]
        public string DosageInstructions { get; init; } = string.Empty;

        // public List<AttachmentRequest>? Attachments { get; init; }
    }

    // public record AttachmentRequest
    // {
    //     [Required(ErrorMessage = "File type is required.")]
    //     public string FileType { get; init; } = string.Empty;

    //     [Required(ErrorMessage = "File URL is required.")]
    //     [Url(ErrorMessage = "File URL must be a valid URL.")]
    //     public string FileUrl { get; init; } = string.Empty;

    //     [Required(ErrorMessage = "Title is required.")]
    //     [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    //     public string Title { get; init; } = string.Empty;

    //     [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    //     public string Description { get; init; } = string.Empty;

    //     public string Visibility { get; init; } = "Private";
    // }
}