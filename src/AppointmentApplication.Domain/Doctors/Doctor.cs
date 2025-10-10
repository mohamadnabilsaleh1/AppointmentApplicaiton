using System;
using System.Collections.Generic;
using System.Linq;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.DoctorDepartments;
using AppointmentApplication.Domain.Doctors.DoctorScheduleExceptions;
using AppointmentApplication.Domain.Doctors.DoctorSchedules;
using AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;
using AppointmentApplication.Domain.Doctors.Enums;
using AppointmentApplication.Domain.Doctors.ScheduleExceptions;
using AppointmentApplication.Domain.Doctors.Schedules;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Domain.Doctors
{
    public class Doctor : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public User? User { get;  set; }
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

        private readonly List<DoctorSchedule> _schedules = new();
        public IReadOnlyCollection<DoctorSchedule> Schedules => _schedules.AsReadOnly();

        private readonly List<DoctorScheduleException> _scheduleExceptions = new();
        public IReadOnlyCollection<DoctorScheduleException> ScheduleExceptions => _scheduleExceptions.AsReadOnly();

        private readonly List<Appointment> _appointments = new();
        public IReadOnlyCollection<Appointment> Appointments => _appointments.AsReadOnly();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        private Doctor() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

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

        public Result<DoctorSchedule> AddSchedule(
        Guid id,
        DaysOfWeek dayOfWeek,
        TimeSpan startTime,
        TimeSpan endTime,
        Status status,
        string? note = null)
        {
            if (_schedules.Any(s => s.DayOfWeek == dayOfWeek))
            {
                return DoctorScheduleErrors.ScheduleAlreadyExistsForDay;
            }

            if (endTime <= startTime)
            {
                return DoctorScheduleErrors.InvalidTimeRange;
            }

            var duration = endTime - startTime;
            if (duration > TimeSpan.FromHours(24))
            {
                return DoctorScheduleErrors.InvalidDuration;
            }

            var scheduleResult = DoctorSchedule.Create(id, dayOfWeek, startTime, endTime, status, note);

            if (scheduleResult.IsError)
            {
                return scheduleResult.Errors;
            }

            var schedule = scheduleResult.Value;

            if (HasScheduleOverlap(schedule))
            {
                return DoctorScheduleErrors.ScheduleOverlap;
            }

            _schedules.Add(schedule);

            return schedule;
        }

        // ✅ Update Schedule
        public Result<Updated> UpdateSchedule(
            Guid scheduleId,
            DaysOfWeek dayOfWeek,
            TimeSpan startTime,
            TimeSpan endTime,
            Status status,
            bool isAvailable,
            string? note = null)
        {
            var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);
            if (schedule == null)
            {
                return DoctorScheduleErrors.ScheduleNotFound;
            }

            // Check if another schedule already exists for the target day (excluding current schedule)
            if (dayOfWeek != schedule.DayOfWeek && _schedules.Any(s => s.Id != scheduleId && s.DayOfWeek == dayOfWeek))
            {
                return DoctorScheduleErrors.ScheduleAlreadyExistsForDay;
            }

            if (endTime <= startTime)
            {
                return DoctorScheduleErrors.InvalidTimeRange;
            }

            var duration = endTime - startTime;
            if (duration > TimeSpan.FromHours(24))
            {
                return DoctorScheduleErrors.InvalidDuration;
            }

            // Create a temporary schedule to check for overlaps
            var tempScheduleResult = DoctorSchedule.Create(
                Guid.NewGuid(), dayOfWeek, startTime, endTime, status, note);

            if (tempScheduleResult.IsError)
            {
                return tempScheduleResult.Errors;
            }

            var tempSchedule = tempScheduleResult.Value;

            // Check for overlaps with other schedules (excluding current schedule)
            if (_schedules.Any(existingSchedule =>
                existingSchedule.Id != scheduleId &&
                existingSchedule.DayOfWeek == dayOfWeek &&
                existingSchedule.IsAvailable &&
                existingSchedule.StartTime < tempSchedule.EndTime &&
                tempSchedule.StartTime < existingSchedule.EndTime))
            {
                return DoctorScheduleErrors.ScheduleOverlap;
            }

            // Update the schedule
            var updateResult = schedule.Update(dayOfWeek, startTime, endTime, status, isAvailable, note);
            if (updateResult.IsError)
            {
                return updateResult.Errors;
            }

            return Result.Updated;
        }

        // ✅ Remove Schedule
        public Result<Deleted> DeleteSchedule(Guid scheduleId)
        {
            var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);
            if (schedule == null)
            {
                return DoctorScheduleErrors.ScheduleNotFound;
            }

            _schedules.Remove(schedule);
            return Result.Deleted;
        }

        // ✅ Get Schedule by Day
        public Result<DoctorSchedule> GetScheduleByDay(DaysOfWeek dayOfWeek)
        {
            var schedule = _schedules.FirstOrDefault(s => s.DayOfWeek == dayOfWeek);
            if (schedule == null)
            {
                return DoctorScheduleErrors.ScheduleNotFoundForDay;
            }

            return schedule;
        }

        // ✅ Get Schedule by ID
        public Result<DoctorSchedule> GetScheduleById(Guid scheduleId)
        {
            var schedule = _schedules.FirstOrDefault(s => s.Id == scheduleId);
            if (schedule == null)
            {
                return DoctorScheduleErrors.ScheduleNotFound;
            }

            return schedule;
        }

        // ✅ Check if facility is open on a specific day and time
        public Result<bool> IsOpenOn(DaysOfWeek dayOfWeek, TimeSpan time)
        {
            var schedule = _schedules.FirstOrDefault(s => s.DayOfWeek == dayOfWeek);
            if (schedule == null || !schedule.IsAvailable)
            {
                return false;
            }

            return time >= schedule.StartTime && time <= schedule.EndTime;
        }

        public Result<DoctorScheduleException> AddScheduleException(
            DateOnly date,
            TimeSpan startTime,
            TimeSpan endTime,
            Status status,
            string? reason = null)
        {
            // Check if exception already exists for this date
            if (_scheduleExceptions.Any(se => se.Date == date))
            {
                return DoctorScheduleExceptionErrors.ExceptionAlreadyExistsForDate;
            }

            if (endTime <= startTime)
            {
                return DoctorScheduleExceptionErrors.InvalidTimeRange;
            }

            var dayOfWeek = GetDayOfWeekFromDate(date);

            var exceptionResult = DoctorScheduleException.Create(
                Id, date, dayOfWeek, startTime, endTime, status, reason);

            if (exceptionResult.IsError)
            {
                return exceptionResult.Errors;
            }

            var scheduleException = exceptionResult.Value;
            _scheduleExceptions.Add(scheduleException);

            return scheduleException;
        }

        public Result<Updated> UpdateScheduleException(
            Guid exceptionId,
            DateOnly date,
            TimeSpan startTime,
            TimeSpan endTime,
            Status status,
            string? reason = null)
        {
            var scheduleException = _scheduleExceptions.FirstOrDefault(se => se.Id == exceptionId);
            if (scheduleException == null)
            {
                return DoctorScheduleExceptionErrors.ScheduleExceptionNotFound;
            }

            // Check if another exception already exists for the target date (excluding current exception)
            if (date != scheduleException.Date && _scheduleExceptions.Any(se => se.Id != exceptionId && se.Date == date))
            {
                return DoctorScheduleExceptionErrors.ExceptionAlreadyExistsForDate;
            }

            if (endTime <= startTime)
            {
                return DoctorScheduleExceptionErrors.InvalidTimeRange;
            }

            var dayOfWeek = GetDayOfWeekFromDate(date);

            var updateResult = scheduleException.Update(date, dayOfWeek, startTime, endTime, status, reason);
            if (updateResult.IsError)
            {
                return updateResult.Errors;
            }

            return Result.Updated;
        }

        public Result<Deleted> DeleteScheduleException(Guid exceptionId)
        {
            var scheduleException = _scheduleExceptions.FirstOrDefault(se => se.Id == exceptionId);
            if (scheduleException == null)
            {
                return DoctorScheduleExceptionErrors.ScheduleExceptionNotFound;
            }

            _scheduleExceptions.Remove(scheduleException);
            return Result.Deleted;
        }

        public Result<DoctorScheduleException> GetScheduleExceptionByDate(DateOnly date)
        {
            var scheduleException = _scheduleExceptions.FirstOrDefault(se => se.Date == date);
            if (scheduleException == null)
            {
                return DoctorScheduleExceptionErrors.ScheduleExceptionNotFoundForDate;
            }

            return scheduleException;
        }

        public Result<DoctorScheduleException> GetScheduleExceptionById(Guid exceptionId)
        {
            var scheduleException = _scheduleExceptions.FirstOrDefault(se => se.Id == exceptionId);
            if (scheduleException == null)
            {
                return DoctorScheduleExceptionErrors.ScheduleExceptionNotFound;
            }

            return scheduleException;
        }

        private static DaysOfWeek GetDayOfWeekFromDate(DateOnly date)
        {
            return date.DayOfWeek switch
            {
                DayOfWeek.Sunday => DaysOfWeek.Sunday,
                DayOfWeek.Monday => DaysOfWeek.Monday,
                DayOfWeek.Tuesday => DaysOfWeek.Tuesday,
                DayOfWeek.Wednesday => DaysOfWeek.Wednesday,
                DayOfWeek.Thursday => DaysOfWeek.Thursday,
                DayOfWeek.Friday => DaysOfWeek.Friday,
                DayOfWeek.Saturday => DaysOfWeek.Saturday,
                _ => DaysOfWeek.Sunday
            };
        }

        private bool HasScheduleOverlap(DoctorSchedule newSchedule)
        {
            return _schedules.Any(existingSchedule =>
                existingSchedule.Id != newSchedule.Id &&
                existingSchedule.DayOfWeek == newSchedule.DayOfWeek &&
                existingSchedule.IsAvailable &&
                existingSchedule.StartTime < newSchedule.EndTime &&
                newSchedule.StartTime < existingSchedule.EndTime);
        }

        public bool HasScheduleExceptionForDate(DateOnly date)
        {
            return _scheduleExceptions.Any(se => se.Date == date);
        }
    }
}


