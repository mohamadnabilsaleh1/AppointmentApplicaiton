using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Patients.Allergies;

namespace AppointmentApplication.Domain.Patients.PatientAllergies;

public class PatientAllergy : AuditableEntity
{
private PatientAllergy() { }
    
    public Guid PatientId { get; private set; }
    public Guid AllergyId { get; private set; }
    
    public Patient Patient { get; private set; }
    public Allergy Allergy { get; private set; }

    public static PatientAllergy Create(Guid patientId, Guid allergyId)
    {
        return new PatientAllergy
        {
            PatientId = patientId,
            AllergyId = allergyId,
        };
    }
}
