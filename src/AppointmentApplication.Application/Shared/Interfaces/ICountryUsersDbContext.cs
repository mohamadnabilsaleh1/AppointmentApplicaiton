using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Citizens;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Shared.Interfaces
{
    public interface ICountryUsersDbContext
    {
        DbSet<Citizen> Citizens { get; set; }
    }
}