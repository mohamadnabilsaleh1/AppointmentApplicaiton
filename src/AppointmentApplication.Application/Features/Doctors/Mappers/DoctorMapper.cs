using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Dtos;
using AppointmentApplication.Application.Features.Emails.Dtos;
using AppointmentApplication.Application.Features.Phones.Dtos;
using AppointmentApplication.Domain.Doctors;

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
                age
            );
        }

        public static List<DoctorDto> ToDtos(this IEnumerable<Doctor> entities)
        {
            return entities.Select(e => e.ToDto()).ToList();
        }

        // New method for DoctorWithContactDto
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
                phones
            );
        }

        public static List<DoctorWithContactDto> ToDtosWithContact(this IEnumerable<Doctor> entities)
        {
            return entities.Select(e => e.ToDtoWithContact()).ToList();
        }
    }
}