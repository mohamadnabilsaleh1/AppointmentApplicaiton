using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.Users.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Patients;

using AppointmentApplication.Domain.Shared.Results;
using AppointmentApplication.Domain.Users;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Users.RegisterPatient
{
    public class RegisterPatientCommandHandler : IRequestHandler<RegisterPatientCommand, Result<Guid>>
    {

        private readonly IAuthenticationService _authenticationService;
        private readonly IAppDbContext _context;
        private readonly ICountryUsersDbContext _countryUsersDbContext;

        // ✅ Constructor Injection
        public RegisterPatientCommandHandler(IAuthenticationService authenticationService, IAppDbContext context, ICountryUsersDbContext countryUsersDbContext)
        {
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _countryUsersDbContext = countryUsersDbContext ?? throw new ArgumentNullException(nameof(countryUsersDbContext));
        }

        public async Task<Result<Guid>> Handle(RegisterPatientCommand request, CancellationToken cancellationToken)
        {
            var citizen = _countryUsersDbContext.Citizens.FirstOrDefault(c => c.NationalId == request.NationalId && c.PhoneNumber == request.PhoneNumber);
            if (citizen == null)
            {
                return ApplicationUserErrors.CitizenNotFound();
            }

            Role role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Patient", cancellationToken) ?? Role.Patient;
            // استخدم Role.Patient مباشرة
            var createUser = User.Create(Guid.NewGuid(), citizen.FirstName, citizen.LastName, request.Email, role);
            if (createUser.IsError)
            {
                return createUser.Errors;
            }

            var user = createUser.Value;
            var existingUser = await _context.Users
           .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (existingUser != null)
            {
                return ApplicationUserErrors.UserAlreadyExists(request.Email);
            }

            string identityId = await _authenticationService.RegisterAsync(user, request.Password, cancellationToken);

            user.SetIdentityId(identityId);
            Result<Patient> createPatientResult = Patient.Create(
                user.Id,
                request.NationalId.ToString(),
                citizen.FirstName,
                citizen.LastName,
                citizen.Gender,
                citizen.BirthDate
                );
            _context.Users.Add(user);
            if (createPatientResult.IsError)
            {
                return createPatientResult.Errors;
            }

            _context.Patients.Add(createPatientResult.Value);
            await _context.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}
