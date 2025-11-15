using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Reviews.Dtos;
using AppointmentApplication.Domain.Reviews;

namespace AppointmentApplication.Application.Features.Reviews.Mappers
{
    public static class ReviewExtensions
    {
        public static HealthCareFacilityReviewDto ToHealthCareFacilityReviewDto(this Review review)
        {
            // Safely handle Specialization conversion
            string doctorSpecialization = review.Doctor?.Specialization.ToString() ?? "Unknown";
            string appointmentStatus = review.Appointment?.Status.ToString() ?? "Unknown";

            return new HealthCareFacilityReviewDto(
                Id: review.Id,
                Rating: review.Rating,
                Comment: review.Comment ?? string.Empty,
                CreatedAtUtc: review.CreatedAtUtc,
                UpdatedAtUtc: review.UpdatedAtUtc,
                AppointmentId: review.AppointmentId,
                PatientId: review.PatientID,
                PatientFirstName: review.Patient?.FirstName ?? "Unknown",
                PatientLastName: review.Patient?.LastName ?? "Patient",
                PatientFullName: $"{review.Patient?.FirstName} {review.Patient?.LastName}",
                PatientEmail: review.Patient?.User?.Email ?? "Unknown",
                DoctorId: review.DoctorID,
                DoctorFirstName: review.Doctor?.FirstName ?? "Unknown",
                DoctorLastName: review.Doctor?.LastName ?? "Doctor",
                DoctorSpecialization: doctorSpecialization,
                DoctorFullName: $"{review.Doctor?.FirstName} {review.Doctor?.LastName}",
                FacilityId: review.FacilityID,
                FacilityName: review.Facility?.Name ?? "Unknown Facility",
                AppointmentDate: review.Appointment?.ScheduledDate,
                AppointmentTime: review.Appointment?.ScheduledTime,
                AppointmentStatus: appointmentStatus
            );
        }
        //    public static List<HealthcareFacilityDto> ToDtos(this IEnumerable<HealthCareFacility> entities)

        public static List<HealthCareFacilityReviewDto> ToHealthCareFacilityReviewDtos(this IEnumerable<Review> reviews)
        {
            /*
                public static List<HealthcareFacilityDto> ToDtos(this IEnumerable<HealthCareFacility> entities)
    {
        return entities.Select(e => e.ToDto()).ToList();
    }

            */
            return reviews.Select(r => r.ToHealthCareFacilityReviewDto()).ToList();
        }
    }
}