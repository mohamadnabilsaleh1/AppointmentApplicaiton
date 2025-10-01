using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Users.Dtos;

using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Application.Features.Users.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(this User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new UserDto(entity.Id, entity.Email, entity.FirstName, entity.LastName);
    }
}
