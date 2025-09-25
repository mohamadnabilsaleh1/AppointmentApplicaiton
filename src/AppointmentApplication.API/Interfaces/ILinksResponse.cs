using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.API.Dtos;

namespace AppointmentApplication.API.Interfaces;

public interface ILinksResponse
{
    List<LinkDto> Links { get; set; }
}