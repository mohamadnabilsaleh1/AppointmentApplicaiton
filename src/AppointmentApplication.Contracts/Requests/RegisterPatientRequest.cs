using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentApplication.Contracts.Requests
{
    public class RegisterPatientRequest
    {
        public string PhoneNumber { get; set; }
        public long NationalId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
