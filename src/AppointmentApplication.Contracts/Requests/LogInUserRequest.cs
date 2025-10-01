using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Contracts.Requests
{
    public class LogInUserRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}