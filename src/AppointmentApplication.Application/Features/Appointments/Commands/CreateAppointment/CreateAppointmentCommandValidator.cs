using System;
using FluentValidation;
using AppointmentApplication.Application.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        private readonly IAppDbContext _context;

        public CreateAppointmentCommandValidator(IAppDbContext context)
        {
            _context = context;

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required")
                .MustAsync(BeValidPatient).WithMessage("Patient must exist and be active");

            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("Doctor ID is required")
                .MustAsync(BeValidDoctor).WithMessage("Doctor must exist and be active");

            RuleFor(x => x.FacilityId)
                .NotEmpty().WithMessage("Facility ID is required")
                .MustAsync(BeValidFacility).WithMessage("Facility must exist and be active");

            RuleFor(x => x.ScheduledDate)
                .NotEmpty().WithMessage("Scheduled date is required")
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("Scheduled date cannot be in the past")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddYears(1)))
                .WithMessage("Scheduled date cannot be more than 1 year in the future");

            RuleFor(x => x.ScheduledTime)
                .NotEmpty().WithMessage("Scheduled time is required")
                .Must(BeWithinBusinessHours).WithMessage("Appointments must be scheduled between 8 AM and 8 PM");

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(15, 480).WithMessage("Duration must be between 15 minutes and 8 hours");

            RuleFor(x => x.Notes)
                .NotEmpty().WithMessage("Notes are required")
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).When(x => x.TotalAmount.HasValue)
                .WithMessage("Total amount must be greater than 0");
        }

        private async Task<bool> BeValidPatient(Guid patientId, CancellationToken cancellationToken)
        {
            return await _context.Patients
                .AnyAsync(p => p.Id == patientId && p.IsActive, cancellationToken);
        }

        private async Task<bool> BeValidDoctor(Guid doctorId, CancellationToken cancellationToken)
        {
            return await _context.Doctors
                .AnyAsync(d => d.Id == doctorId && d.IsActive, cancellationToken);
        }

        private async Task<bool> BeValidFacility(Guid facilityId, CancellationToken cancellationToken)
        {
            return await _context.HealthcareFacilities
                .AnyAsync(f => f.Id == facilityId && f.IsActive, cancellationToken);
        }

        private bool BeWithinBusinessHours(TimeSpan time)
        {
            return time >= TimeSpan.FromHours(8) && time <= TimeSpan.FromHours(20);
        }
    }
}