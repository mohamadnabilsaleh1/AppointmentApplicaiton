using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Patients.Allergies.Enums;

namespace AppointmentApplication.Contracts.Requests.Patients.Allergies
{
    public class CreateAllergy
    {
        public AllergyType Allergy { get; set; }
    }
}