using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Application.Features.Emails.Dtos;
using AppointmentApplication.Application.Features.Phones.Dtos;
using AppointmentApplication.Domain.Doctors;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AppointmentApplication.Application.Features.Doctors.Mappers
{
    public static class DoctorMapper
    {
        public static DoctorDto ToDto(this Doctor entity)
        {
            int age = Doctor.CalculateAge(entity.DateOfBirth);
            return new DoctorDto(
                entity.Id,
                entity.FacilityId,
                entity.FirstName,
                entity.LastName,
                entity.Gender,
                entity.Specialization,
                entity.Description,
                age,
                entity.User.Avatar == string.Empty ? "" : "api/users/" + entity.User.Id.ToString() + "/avatar"
            );
        }
        public static DepartmentDoctorsDto DepartmentDoctorsToDto(this Doctor entity)
        {
            int age = Doctor.CalculateAge(entity.DateOfBirth);
            return new DepartmentDoctorsDto(
                entity.Id,
                entity.FacilityId,
                entity.FirstName,
                entity.LastName,
                entity.Gender,
                entity.Specialization,
                age,
                entity.Description

            );
        }

        public static List<DoctorDto> ToDtos(this IEnumerable<Doctor> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }

        // Updated method for DoctorWithContactDto with review statistics
        public static DoctorWithContactDto ToDtoWithContact(this Doctor entity)
        {
            int age = Doctor.CalculateAge(entity.DateOfBirth);

            var primaryEmail = entity.User?.GetPrimaryEmail();
            var primaryPhone = entity.User?.GetPrimaryPhone();

            var emails = entity.User?.Emails
                .Select(e => new EmailDto(e.Id, e.EmailAddress, e.Label, e.IsPrimary))
                .ToList() ?? new List<EmailDto>();

            var phones = entity.User?.Phones
                .Select(p => new PhoneDto(p.Id, p.PhoneNumber, p.Label, p.IsPrimary))
                .ToList() ?? new List<PhoneDto>();

            // ✅ Calculate review statistics
            var (averageRating, totalReviews, positiveReviews, positiveReviewPercentage) = CalculateReviewStatistics(entity);

            return new DoctorWithContactDto(
                entity.Id,
                entity.FacilityId,
                entity.FirstName,
                entity.LastName,
                entity.Gender,
                entity.Specialization,
                age,
                primaryEmail?.EmailAddress,
                primaryPhone?.PhoneNumber,
                emails,
                phones,
                entity.User.Avatar == string.Empty ? "" : "api/users/" + entity.User.Id.ToString() + "/avatar",
                averageRating,
                totalReviews,
                positiveReviews,
                positiveReviewPercentage
            );
        }

        public static List<DoctorWithContactDto> ToDtosWithContact(this IEnumerable<Doctor> entities)
        {
            return entities.Select(e => e.ToDtoWithContact()).ToList();
        }
        public static List<DepartmentDoctorsDto> DepartmentDoctorsToDtos(this IEnumerable<Doctor> entities)
        {
            return entities.Select(e => e.DepartmentDoctorsToDto()).ToList();
        }

        // ✅ Helper method to calculate review statistics
        private static (double averageRating, int totalReviews, int positiveReviews, double positiveReviewPercentage)
            CalculateReviewStatistics(Doctor doctor)
        {
            if (doctor.Reviews == null || !doctor.Reviews.Any())
            {
                return (0, 0, 0, 0);
            }

            var reviews = doctor.Reviews.ToList();
            var totalReviews = reviews.Count;
            var averageRating = Math.Round(reviews.Average(r => r.Rating), 1);
            var positiveReviews = reviews.Count(r => r.Rating >= 4);
            var positiveReviewPercentage = totalReviews > 0 ?
                Math.Round((double)positiveReviews / totalReviews * 100, 1) : 0;

            return (averageRating, totalReviews, positiveReviews, positiveReviewPercentage);
        }

        // ✅ Alternative method if you want to use the domain method
        public static DoctorWithContactDto ToDtoWithContactAndReviews(this Doctor entity)
        {
            var dto = entity.ToDtoWithContact();

            // If you have the GetReviewStatistics method in your Doctor entity
            // var statistics = entity.GetReviewStatistics();
            // Then map the statistics to the DTO

            return dto;
        }
    }
}