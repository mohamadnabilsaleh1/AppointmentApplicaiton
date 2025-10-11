using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Patients.Allergies.Enums;
using AppointmentApplication.Domain.Patients.ChronicDiseases.Enums;

namespace AppointmentApplication.Contracts.Requests.Patients.ChronicDiseases
{
    public class CreateChronicDisease
    {
        public ChronicDiseaseType ChronicDisease { get; set; }
    }
}