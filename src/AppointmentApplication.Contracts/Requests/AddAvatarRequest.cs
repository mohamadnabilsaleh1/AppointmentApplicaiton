using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace AppointmentApplication.Contracts.Requests
{
    public class AddAvatarRequest
    {
        public IFormFile File { get; set; }
    }
}