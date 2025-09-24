using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Patients.Allergies.Enums;
using AppointmentApplication.Domain.Patients.PatientAllergies;

namespace AppointmentApplication.Domain.Patients.Allergies;

public class Allergy : AuditableEntity
{
    public AllergyType Name { get; private set; }
    private readonly List<PatientAllergy> _patientAllergies = new();
    public IReadOnlyCollection<PatientAllergy> PatientAllergies => _patientAllergies.AsReadOnly();
}
