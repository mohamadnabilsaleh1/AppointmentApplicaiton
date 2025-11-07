using System.Collections.Generic;
using System.Linq;
using AppointmentApplication.Application.Features.Appointments.Dtos;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Patients;

namespace AppointmentApplication.Application.Features.Appointments.Mappers
{
    public static class AppointmentMapper
    {
        public static AppointmentDto ToDto(this Appointment appointment)
        {
            return new AppointmentDto(
                Id: appointment.Id,
                ScheduledDate: appointment.ScheduledDate,
                ScheduledTime: appointment.ScheduledTime,
                DurationMinutes: appointment.DurationMinutes,
                Status: appointment.Status,
                BookingDate: appointment.BookingDate,
                Notes: appointment.Notes ?? string.Empty,
                Patient: MapPatient(appointment.Patient),
                Doctor: MapDoctor(appointment.Doctor),
                Facility: MapFacility(appointment.Facility)
            );
        }

        private static PatientInfoDto MapPatient(Patient patient)
        {
            if (patient == null)
            {
                return null;
            }

            return new PatientInfoDto(
                Id: patient.Id,
                FullName: $"{patient.FirstName} {patient.LastName}".Trim(),
                NationalID: patient.NationalID
            );
        }

        private static DoctorInfoDto MapDoctor(Doctor doctor)
        {
            if (doctor == null)
            {
                return null;
            }

            return new DoctorInfoDto(
                Id: doctor.Id,
                FullName: $"{doctor.FirstName} {doctor.LastName}".Trim(),
                Specialization: doctor.Specialization.ToString()
            );
        }

        private static FacilityInfoDto MapFacility(HealthCareFacility facility)
        {
            if (facility == null)
            {
                return null;
            }

            string fullAddress = $"{facility.Address.Street}, {facility.Address.City}, {facility.Address.Country} {facility.Address.ZipCode}";
            return new FacilityInfoDto(
                Id: facility.Id,
                Name: facility.Name,
                Address: fullAddress ?? string.Empty
            );
        }

        public static List<AppointmentDto> ToDtos(this IEnumerable<Appointment> appointments)
        {
            return appointments.Select(ToDto).ToList();
        }
    }
}