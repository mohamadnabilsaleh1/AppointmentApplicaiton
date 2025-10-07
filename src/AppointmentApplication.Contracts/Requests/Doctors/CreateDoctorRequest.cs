using System;
using System.ComponentModel.DataAnnotations;

using AppointmentApplication.Domain.Doctors.Enums;
using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Contracts.Requests.Doctors
{
    public class CreateDoctorRequest
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Specialization is required")]
        public Specialization Specialization { get; set; }

        [Required(ErrorMessage = "License number is required")]
        [StringLength(50)]
        public string LicenseNumber { get; set; } = string.Empty;

    }
}
