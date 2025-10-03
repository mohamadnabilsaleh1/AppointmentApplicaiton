using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.API.Dtos.HealthCareFacilities
{
    public class DoctorQueryParameters : QueryParameters
    {
        public Guid? FacilityId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? SpecializationId { get; set; }
        public Gender? Gender { get; set; }
        public bool? IsActive { get; set; }
    }
}