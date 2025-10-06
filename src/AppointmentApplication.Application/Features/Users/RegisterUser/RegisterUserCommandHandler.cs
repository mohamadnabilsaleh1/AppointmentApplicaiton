using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Abstractions;

using AppointmentApplication.Domain.Shared.Results;

using AppointmentApplication.Domain.Users;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IAppDbContext _context;

    // ✅ Constructor Injection
    public RegisterUserCommandHandler(IAuthenticationService authenticationService, IAppDbContext context)
    {
        _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        Role role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin", cancellationToken) ?? Role.Admin;
        // استخدم Role.Patient مباشرة
        var createUser = User.Create(Guid.NewGuid(), request.FirstName, request.LastName, request.Email, role);
        var user = createUser.Value;

        string identityId = await _authenticationService.RegisterAsync(user, request.Password, cancellationToken);

        user.SetIdentityId(identityId);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}
