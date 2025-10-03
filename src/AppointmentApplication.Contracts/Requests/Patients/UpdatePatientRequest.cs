using System;
using System.ComponentModel.DataAnnotations;

using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Contracts.Requests.Patient
{
    public class UpdatePatientRequest
    {
        [Required(ErrorMessage = "National ID is required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "National ID must be between 5 and 20 characters")]
        public string NationalID { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}