using System;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;

namespace AppointmentApplication.API.Dtos.HealthCareFacilities
{
    public class HealthCareFacilityQueryParameters : QueryParameters
    {
        public double? GPSLatitude { get; set; }
        public double? GPSLongitude { get; set; }
        public HealthCareType? Type { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
    }
}
