using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentApplication.Contracts.Requests.HealthCareFacilities
{
    public class UpdateHealthcareFacilityRequest
    {
        [Required(ErrorMessage = "Facility name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Facility name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Street address is required")]
        [StringLength(200, ErrorMessage = "Street address cannot exceed 200 characters")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City name cannot exceed 50 characters")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State name cannot exceed 50 characters")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country name cannot exceed 50 characters")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Zip code is required")]
        [StringLength(20, ErrorMessage = "Zip code cannot exceed 20 characters")]
        public string ZipCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "GPS latitude is required")]
        [Range(-90.0, 90.0, ErrorMessage = "GPS latitude must be between -90 and 90")]
        public double GPSLatitude { get; set; }

        [Required(ErrorMessage = "GPS longitude is required")]
        [Range(-180.0, 180.0, ErrorMessage = "GPS longitude must be between -180 and 180")]
        public double GPSLongitude { get; set; }
    }
}