using System;
using System.Collections.Generic;
using System.Linq;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.DoctorDepartments;
using AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;
using AppointmentApplication.Domain.Doctors.Enums;
using AppointmentApplication.Domain.Doctors.ScheduleExceptions;
using AppointmentApplication.Domain.Doctors.Schedules;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Domain.Doctors
{
    public class Doctor : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;
        public Guid FacilityId { get; private set; }
        public HealthCareFacility? HealthcareFacility { get; set; }

        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;
        public Gender Gender { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public string LicenseNumber { get; private set; } = null!;
        public bool IsActive { get; private set; } = true;
        public Specialization Specialization { get; private set; }
        public DoctorTreatmentCapacity? TreatmentCapacity { get; private set; }

        private readonly List<DoctorDepartment> _departments = new();
        public IReadOnlyCollection<DoctorDepartment> Departments => _departments.AsReadOnly();

        private readonly List<ScheduleDoctor> _schedules = new();
        public IReadOnlyCollection<ScheduleDoctor> Schedules => _schedules.AsReadOnly();

        private readonly List<ScheduleExceptionDoctor> _scheduleExceptions = new();
        public IReadOnlyCollection<ScheduleExceptionDoctor> ScheduleExceptions => _scheduleExceptions.AsReadOnly();

        private readonly List<Appointment> _appointments = new();
        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

        private Doctor() { }

        private Doctor(
            Guid id,
            Guid facilityId,
            Guid userId,
            string firstName,
            string lastName,
            Gender gender,
            DateOnly dateOfBirth,
            Specialization specialization,
            string licenseNumber)
            : base(id)
        {
            UserId = userId;
            FacilityId = facilityId;
            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            LicenseNumber = licenseNumber;
            Specialization = specialization;
        }

        // ✅ Factory Method
        public static Result<Doctor> Create(
            Guid userId,
            Guid facilityId,
            string firstName,
            string lastName,
            Gender gender,
            DateOnly dateOfBirth,
            Specialization specialization,
            string licenseNumber)
        {
            // Domain validation
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {

                return DoctorErrors.InvalidName;
            }

            if (string.IsNullOrWhiteSpace(licenseNumber))
            {

                return DoctorErrors.InvalidLicenseNumber;
            }

            if (dateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow))
            {

                return DoctorErrors.InvalidDateOfBirth;
            }

            if (!Enum.IsDefined(typeof(Gender), gender))
            {

                return DoctorErrors.InvalidGender;
            }

            if (!Enum.IsDefined(typeof(Specialization), specialization))
            {

                return DoctorErrors.InvalidSpecialization;
            }

            var doctor = new Doctor(Guid.NewGuid(), facilityId, userId, firstName, lastName, gender, dateOfBirth, specialization, licenseNumber);
            return doctor;
        }

        // ✅ Update Method
        public Result<Updated> Update(
            string firstName,
            string lastName,
            Gender gender,
            DateOnly dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
            {

                return DoctorErrors.InvalidName;
            }



            if (dateOfBirth >= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return DoctorErrors.InvalidDateOfBirth;
            }

            if (!Enum.IsDefined(typeof(Gender), gender))
            {

                return DoctorErrors.InvalidGender;
            }

            FirstName = firstName;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            UpdatedAtdUtc = DateTime.UtcNow;

            return Result.Updated;
        }

        // ✅ Activate / Deactivate
        public Result<Updated> Activate()
        {
            if (IsActive)
            {
                return Result.Updated; // No change, but still valid
            }

            IsActive = true;
            UpdatedAtdUtc = DateTime.UtcNow;
            return Result.Updated;
        }

        public Result<Updated> Deactivate()
        {
            if (!IsActive)
            {
                return Result.Updated;
            }

            IsActive = false;
            UpdatedAtdUtc = DateTime.UtcNow;
            return Result.Updated;
        }
        public static int CalculateAge(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            int age = today.Year - dateOfBirth.Year;

            // If the birthday hasn't occurred this year yet, subtract 1
            if (dateOfBirth > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}


