using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Patients.ChronicDiseases;

namespace AppointmentApplication.Domain.Patients.PatientChronicDiseases;

public class PatientChronicDisease : AuditableEntity
{
    private PatientChronicDisease() { }
    
    public Guid PatientId { get; private set; }
    public Guid ChronicDiseaseId { get; private set; }
    
    public Patient Patient { get; private set; }
    public ChronicDisease ChronicDisease { get; private set; }

    public static PatientChronicDisease Create(Guid patientId, Guid chronicDiseaseId)
    {
        return new PatientChronicDisease
        {
            Id = Guid.NewGuid(),
            PatientId = patientId,
            ChronicDiseaseId = chronicDiseaseId,
        };
    }

}
