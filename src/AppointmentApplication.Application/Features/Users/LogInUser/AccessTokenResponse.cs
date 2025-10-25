// Application/Features/Users/LogInUser/AccessTokenResponse.cs
using AppointmentApplication.Application.Features.Users.Dtos;

namespace AppointmentApplication.Application.Features.Users.LogInUser;

public sealed record AccessTokenResponse(
    string AccessToken,
    UserDto User);