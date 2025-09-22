using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.MedicalRecords;
using AppointmentApplication.Domain.Patients.PatientAllergies;
using AppointmentApplication.Domain.Patients.PatientChronicDiseases;
using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Domain.Patients;

public class Patient : AuditableEntity
{
    private Patient() { }

    public Guid UserID { get; private set; }
    public string NationalID { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Gender { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string BloodType { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<PatientAllergy> _allergies = new();
    public IReadOnlyCollection<PatientAllergy> Allergies => _allergies.AsReadOnly();

    private readonly List<PatientChronicDisease> _chronicDiseases = new();
    public IReadOnlyCollection<PatientChronicDisease> ChronicDiseases => _chronicDiseases.AsReadOnly();

    private readonly List<Appointment> _appointments = new();
    public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

    private readonly List<MedicalRecord> _medicalRecords = new();
    public IReadOnlyCollection<MedicalRecord> MedicalRecords => _medicalRecords.AsReadOnly();

    public static Patient Create(Guid userId, string nationalId, string firstName, string lastName,
        string gender, DateTime dateOfBirth, string bloodType)
    {
        return new Patient
        {
            Id = Guid.NewGuid(),
            UserID = userId,
            NationalID = nationalId,
            FirstName = firstName,
            LastName = lastName,
            Gender = gender,
            DateOfBirth = dateOfBirth,
            BloodType = bloodType,
        };
    }

    public void Update(string firstName, string lastName, string gender, DateTime dateOfBirth, string bloodType)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        BloodType = bloodType;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
