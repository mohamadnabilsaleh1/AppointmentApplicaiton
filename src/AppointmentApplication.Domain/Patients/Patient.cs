using System;
using System.Collections.Generic;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.MedicalRecords;
using AppointmentApplication.Domain.Patients.Allergies;
using AppointmentApplication.Domain.Patients.ChronicDiseases;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Domain.Patients
{
    public class Patient : AuditableEntity
    {
        private Patient() { }

        public Guid UserId { get; private set; }
        public User? User { get; set; }
        public string NationalID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Gender Gender { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public bool IsActive { get; private set; }

        // 👇 Direct many-to-many
        public ICollection<Allergy> Allergies { get; private set; } = new List<Allergy>();
        public ICollection<ChronicDisease> ChronicDiseases { get; private set; } = new List<ChronicDisease>();

        public ICollection<Appointment> Appointments { get; private set; } = new List<Appointment>();
        public ICollection<MedicalRecord> MedicalRecords { get; private set; } = new List<MedicalRecord>();

        public Patient(Guid userId, string nationalId, string firstName, string lastName, Gender gender, DateOnly dateOfBirth)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            NationalID = nationalId;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            IsActive = true;
        }

        public static Result<Patient> Create(Guid userId, string nationalId, string firstName, string lastName, Gender gender, DateOnly dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {
                return PatientErrors.InvalidName;
            }


            if (string.IsNullOrWhiteSpace(nationalId))
            {
                return PatientErrors.NationalId;
            }


            if (dateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return PatientErrors.InvalidDateOfBirth;
            }


            if (!Enum.IsDefined(typeof(Gender), gender))
            {
                return PatientErrors.InvalidGender;
            }


            return new Patient(userId, nationalId, firstName, lastName, gender, dateOfBirth);
        }

        public Result<Updated> Update(string nationalId, Gender gender, DateOnly dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
            {
                return PatientErrors.NationalId;
            }


            if (dateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return PatientErrors.InvalidDateOfBirth;
            }


            if (!Enum.IsDefined(typeof(Gender), gender))
            {
                return PatientErrors.InvalidGender;
            }


            NationalID = nationalId;
            Gender = gender;
            DateOfBirth = dateOfBirth;

            return Result.Updated;
        }

        public Result<Updated> Deactivate()
        {
            IsActive = false;
            return Result.Updated;
        }

        public Result<Updated> Activate()
        {
            IsActive = true;
            return Result.Updated;
        }

        public Result<Updated> AddAllergy(Allergy allergy)
        {
            if (Allergies.Contains(allergy))
            {
                return PatientErrors.AllergyAlreadyExists;
            }

            Allergies.Add(allergy);
            return Result.Updated;
        }

        public Result<Deleted> DeleteAllergy(Allergy allergy)
        {
            if (!Allergies.Contains(allergy))
            {
                return PatientErrors.AllergyNotFound;
            }

            Allergies.Remove(allergy);
            return Result.Deleted;
        }

        public Result<Updated> AddChronicDiseases(ChronicDisease chronicDisease)
        {
            if (ChronicDiseases.Contains(chronicDisease))
            {
                return PatientErrors.AllergyAlreadyExists;
            }

            ChronicDiseases.Add(chronicDisease);
            return Result.Updated;
        }
        public Result<Deleted> DeleteChronicDisease(ChronicDisease chronicDisease)
        {
            if (!ChronicDiseases.Contains(chronicDisease))
            {
                return PatientErrors.ChronicDiseaseNotFound;
            }

            ChronicDiseases.Remove(chronicDisease);
            return Result.Deleted;
        }
    }
}
