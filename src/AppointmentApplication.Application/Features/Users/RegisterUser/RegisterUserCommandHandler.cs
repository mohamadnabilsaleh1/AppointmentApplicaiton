using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Abstractions.Authentication;

using AppointmentApplication.Domain.Abstractions;

using AppointmentApplication.Domain.Shared.Results;

using AppointmentApplication.Domain.Users;

using MediatR;

namespace AppointmentApplication.Application.Features.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IAuthenticationService authenticationService,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = User.Create(
            Guid.NewGuid(),
            new FirstName(request.FirstName),
            new LastName(request.LastName),
            new Email(request.Email));

        string identityId = await _authenticationService.RegisterAsync(
            user,
            request.Password,
            cancellationToken);

        user.SetIdentityId(identityId);

        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }


}