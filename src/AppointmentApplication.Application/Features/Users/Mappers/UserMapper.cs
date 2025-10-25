// Application/Features/Users/Mappers/UserMapper.cs
using AppointmentApplication.Application.Features.Users.Dtos;
using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Application.Features.Users.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(this User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        // Get the first role or null if no roles exist
        var role = entity.Roles.FirstOrDefault()?.Name; // Assuming Role has a Name property

        return new UserDto(
            entity.Id,
            entity.Email,
            entity.FirstName,
            entity.LastName,
            role!);
    }
}