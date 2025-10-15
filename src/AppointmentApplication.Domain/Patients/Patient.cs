using System;
using System.Collections.Generic;
using System.Linq;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.MediaUploads;
using AppointmentApplication.Domain.MediaUploads.Enums;
using AppointmentApplication.Domain.MedicalRecords;
using AppointmentApplication.Domain.Patients.Allergies;
using AppointmentApplication.Domain.Patients.Allergies.Enums;
using AppointmentApplication.Domain.Patients.ChronicDiseases;
using AppointmentApplication.Domain.Patients.ChronicDiseases.Enums;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Domain.Patients
{
    public class Patient : AuditableEntity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        private Patient() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public Guid UserId { get; private set; }
        public User? User { get; private set; }
        public string NationalID { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Gender Gender { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public bool IsActive { get; private set; }

        // ✅ Private fields for encapsulation
        private readonly List<Allergy> _allergies = new();
        public IReadOnlyCollection<Allergy> Allergies => _allergies.AsReadOnly();

        private readonly List<ChronicDisease> _chronicDiseases = new();
        public IReadOnlyCollection<ChronicDisease> ChronicDiseases => _chronicDiseases.AsReadOnly();

        private readonly List<Appointment> _appointments = new();
        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

        private readonly List<MedicalRecord> _medicalRecords = new();
        public IReadOnlyCollection<MedicalRecord> MedicalRecords => _medicalRecords.AsReadOnly();

        private readonly List<PatientUpload> _uploads = new();
        public IReadOnlyCollection<PatientUpload> Uploads => _uploads.AsReadOnly();

        // ✅ Constructor
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

        // ✅ Factory Method
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

        // ✅ Update methods
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

        // ✅ Domain logic
        public Result<Allergy> AddAllergy(AllergyType allergyType)
        {
            var allergyExist = _allergies.FirstOrDefault(a => a.Name == allergyType);
            if (allergyExist != null)
            {
                return PatientErrors.AllergyAlreadyExists;
            }

            // Get the predefined allergy instance
            var allergy = Allergy.GetAllergyByType(allergyType);
            if (allergy == null)
            {
                return PatientErrors.InvalidAllergyType;
            }
            _allergies.Add(allergy);
            return allergy;
        }


        public Result<Deleted> DeleteAllergy(Guid allergyId)
        {
            var allergy = _allergies.FirstOrDefault(a => a.Id == allergyId);
            if (allergy == null)
            {
                return PatientErrors.AllergyNotFound;
            }

            _allergies.Remove(allergy);
            return Result.Deleted;
        }

        public Result<ChronicDisease> AddChronicDisease(ChronicDiseaseType chronicDiseaseType)
        {
            var existing = _chronicDiseases.FirstOrDefault(cd => cd.Name == chronicDiseaseType);
            if (existing != null)
            {
                return PatientErrors.ChronicDiseaseAlreadyExists;
            }

            // Get the predefined chronic disease instance
            var chronicDisease = ChronicDisease.GetChronicDiseaseByType(chronicDiseaseType);
            if (chronicDisease == null)
            {
                return PatientErrors.InvalidChronicDiseaseType;
            }

            _chronicDiseases.Add(chronicDisease);
            return chronicDisease;
        }

        public Result<Deleted> DeleteChronicDisease(Guid chronicDiseaseId)
        {
            var chronicDisease = _chronicDiseases.FirstOrDefault(cd => cd.Id == chronicDiseaseId);
            if (chronicDisease == null)
            {
                return PatientErrors.ChronicDiseaseNotFound;
            }

            _chronicDiseases.Remove(chronicDisease);
            return Result.Deleted;
        }

        public Result<PatientUpload> AddUpload(Guid patientId, string fileType, string fileUrl,
            string title, string description, Visibility visibility = Visibility.Public)
        {
            var uploadResult = PatientUpload.Create(patientId, fileType, fileUrl, title, description, visibility);
            if (uploadResult.IsError)
            {
                return uploadResult.Errors;
            }

            _uploads.Add(uploadResult.Value);
            return uploadResult.Value;
        }
        public Result<Updated> UpdateUpload(Guid uploadId, string title, string description)
        {
            var upload = _uploads.FirstOrDefault(u => u.Id == uploadId);
            if (upload == null)
            {
                return PatientErrors.UploadNotFound;
            }

            var updateResult = upload.Update(title, description);
            if (updateResult.IsError)
            {
                return updateResult.Errors;
            }

            return Result.Updated;
        }
        public Result<Updated> ChangeUploadVisibilityToPublic(Guid uploadId)
        {
            var upload = _uploads.FirstOrDefault(u => u.Id == uploadId);
            if (upload == null)
            {
                return PatientErrors.UploadNotFound;
            }

            var changeResult = upload.ChangeVisibilityToPublic();
            if (changeResult.IsError)
            {
                return changeResult.Errors;
            }

            return Result.Updated;
        }
        public Result<Updated> ChangeUploadVisibilityToPrivate(Guid uploadId)
        {
            var upload = _uploads.FirstOrDefault(u => u.Id == uploadId);
            if (upload == null)
            {
                return PatientErrors.UploadNotFound;
            }

            var changeResult = upload.ChangeVisibilityToPrivate();
            if (changeResult.IsError)
            {
                return changeResult.Errors;
            }

            return Result.Updated;
        }
        public Result<PatientUpload> GetUploadedById(Guid uploadId)
        {
            var upload = _uploads.FirstOrDefault(u => u.Id == uploadId);
            if (upload == null)
            {
                return PatientErrors.UploadNotFound;
            }
            return upload;
        }
        public Result<Deleted> DeleteUploadedFile(Guid uploadId)
        {
            var uploadedFiles = _uploads.FirstOrDefault(cd => cd.Id == uploadId);
            if (uploadedFiles == null)
            {
                return PatientErrors.UploadNotFound;
            }

            _uploads.Remove(uploadedFiles);
            return Result.Deleted;
        }
    }
}
