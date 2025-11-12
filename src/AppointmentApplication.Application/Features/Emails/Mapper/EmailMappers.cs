using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Emails.Dtos;
using AppointmentApplication.Domain.Emails;

namespace AppointmentApplication.Application.Features.Emails.Mapper
{
    public static class EmailMappers
    {
        public static EmailDto ToDto(this Email email)
        {
            return new EmailDto(
                Id: email.Id,
                EmailAddress: email.EmailAddress,
                Label: email.Label,
                IsPrimary: email.IsPrimary
            );
        }
    }
}