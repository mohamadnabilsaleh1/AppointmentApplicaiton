using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Phones.Dtos;
using AppointmentApplication.Domain.Phones;

namespace AppointmentApplication.Application.Features.Phones.Mapper
{
 public static class PhoneMappers
    {
        public static PhoneDto ToDto(this Phone phone)
        {
            return new PhoneDto(
                Id: phone.Id,
                PhoneNumber: phone.PhoneNumber,
                Label: phone.Label,
                IsPrimary: phone.IsPrimary
            );
        }
    }
}