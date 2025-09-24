using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Patients.ChronicDiseases.Enums;
using AppointmentApplication.Domain.Patients.PatientChronicDiseases;

namespace AppointmentApplication.Domain.Patients.ChronicDiseases;

public class ChronicDisease : AuditableEntity
{
    public ChronicDiseaseType Name { get; private set; }

    private readonly List<PatientChronicDisease> _patientChronicDiseases = new();
    public IReadOnlyCollection<PatientChronicDisease> PatientChronicDiseases => _patientChronicDiseases.AsReadOnly();
}
