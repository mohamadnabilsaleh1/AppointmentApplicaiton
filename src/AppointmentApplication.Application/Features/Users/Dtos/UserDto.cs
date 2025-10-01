using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Application.Features.Users.Dtos;

public sealed record UserDto(Guid Id, string Email, string FirstName, string LastName);