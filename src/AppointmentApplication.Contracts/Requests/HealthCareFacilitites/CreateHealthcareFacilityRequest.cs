using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;

namespace AppointmentApplication.Contracts.Requests;

public class CreateHealthcareFacilityRequest
{
    // User details
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    // Facility details
    [Required(ErrorMessage = "Facility name is required")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "Facility name must be between 2 and 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Facility type is required")]
    public HealthCareType Type { get; set; }

    [Required(ErrorMessage = "Street address is required")]
    [StringLength(150, ErrorMessage = "Street address cannot exceed 150 characters")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required")]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required")]
    [StringLength(100, ErrorMessage = "State cannot exceed 100 characters")]
    public string State { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required")]
    [StringLength(100, ErrorMessage = "Country cannot exceed 100 characters")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "Zip code is required")]
    [StringLength(20, ErrorMessage = "Zip code cannot exceed 20 characters")]
    public string ZipCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "GPS latitude is required")]
    [Range(-90, 90, ErrorMessage = "GPS latitude must be between -90 and 90 degrees")]
    public double GPSLatitude { get; set; }

    [Required(ErrorMessage = "GPS longitude is required")]
    [Range(-180, 180, ErrorMessage = "GPS longitude must be between -180 and 180 degrees")]
    public double GPSLongitude { get; set; }
}
