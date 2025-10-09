using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Application.Features.Doctors.Errors;
using AppointmentApplication.Application.Features.Doctors.Mappers;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppointmentApplication.Application.Features.Doctors.Commands.CreateDoctor
{
    public class CreateDoctorCommandHandler(
        ILogger<CreateDoctorCommandHandler> logger,
        IAppDbContext context,
        IAuthenticationService authenticationService)
        : IRequestHandler<CreateDoctorCommand, Result<DoctorDto>>
    {
        private readonly ILogger<CreateDoctorCommandHandler> _logger = logger;
        private readonly IAppDbContext _context = context;
        private readonly IAuthenticationService _authenticationService = authenticationService;

        public async Task<Result<DoctorDto>> Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
        {
            // 1️⃣ Find the Healthcare Facility owned by this User
            var facility = await _context.HealthcareFacilities
                .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

            if (facility == null)
            {
                _logger.LogWarning("Doctor creation failed. Facility not found for UserId: {UserId}", request.UserId);
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }

            if (!facility.IsActive)
            {
                _logger.LogWarning("Doctor creation failed. Facility {FacilityName} is inactive.", facility.Name);
                return ApplicationDoctorErrors.FacilityNotActive(facility.Name);
            }

            // 2️⃣ Check if user already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (existingUser != null)
            {
                _logger.LogWarning("Doctor creation aborted. User with email {Email} already exists.", request.Email);
                return ApplicationDoctorErrors.UserAlreadyExists(request.Email);
            }

            // 3️⃣ Get doctor role
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == "Doctor", cancellationToken)
                ?? Role.Doctor;

            // 4️⃣ Create User domain entity
            var createUserResult = User.Create(Guid.NewGuid(), request.FirstName, request.LastName, request.Email, role);
            if (createUserResult.IsError)
            {
                _logger.LogWarning("User creation failed: {Errors}", string.Join(", ", createUserResult.Errors));
                return createUserResult.Errors;
            }

            var user = createUserResult.Value;

            // 5️⃣ Register user in authentication system
            string identityId = await _authenticationService.RegisterAsync(user, request.Password, cancellationToken);
            user.SetIdentityId(identityId);

            // 6️⃣ Create Doctor domain entity linked to the Facility
            var createDoctorResult = facility.AddDoctor(
                user.Id,
                request.FirstName,
                request.LastName,
                request.Gender,
                request.DateOfBirth,
                request.Specialization,
                request.LicenseNumber
            );

            if (createDoctorResult.IsError)
            {
                _logger.LogWarning("Doctor creation failed: {Errors}", string.Join(", ", createDoctorResult.Errors));
                return createDoctorResult.Errors;
            }

            var doctor = createDoctorResult.Value;

            // 7️⃣ Save User + Doctor
            _context.Users.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Doctor created successfully for Facility {FacilityName}. DoctorId: {DoctorId}, UserId: {UserId}, Email: {Email}",
                facility.Name, doctor.Id, user.Id, request.Email);

            return doctor.ToDto();
        }
    }
}
