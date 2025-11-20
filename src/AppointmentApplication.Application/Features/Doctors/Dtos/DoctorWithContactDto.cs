using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Emails.Dtos;
using AppointmentApplication.Application.Features.Phones.Dtos;
using AppointmentApplication.Domain.Doctors.Enums;
using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Application.Features.Doctors.Dtos
{
    public record DoctorWithContactDto(
        Guid Id,
        Guid HealthCareFacilityId,
        string FirstName,
        string LastName,
        Gender Gender,
        Specialization Specialization,
        int Age,
        string? PrimaryEmail,
        string? PrimaryPhone,
        List<EmailDto> Emails,
        List<PhoneDto> Phones,
        string Avatar,
        double AverageRating,
        int TotalReviews,
        int PositiveReviews,
        double PositiveReviewPercentage
    );
}