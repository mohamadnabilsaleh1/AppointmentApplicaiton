using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Contracts.Requests.Doctors.DoctorsTreatmentCapabilities
{
    public class UpdateDoctorsTreatmentCapacityRequest
    {
        [Required(ErrorMessage = "Max patients per day is required")]
        [Range(1, 500, ErrorMessage = "Max patients per day must be between 1 and 500")]
        public int MaxPatientsPerDay { get; set; }

        [Required(ErrorMessage = "Session duration is required")]
        [Range(5, 480, ErrorMessage = "Session duration must be between 5 and 480 minutes")]
        public int SessionDurationMinutes { get; set; }
    }
}