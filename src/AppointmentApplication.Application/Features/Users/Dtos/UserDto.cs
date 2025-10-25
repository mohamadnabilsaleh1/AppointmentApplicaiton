// Application/Features/Users/Dtos/UserDto.cs
namespace AppointmentApplication.Application.Features.Users.Dtos;

public sealed record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string Role);