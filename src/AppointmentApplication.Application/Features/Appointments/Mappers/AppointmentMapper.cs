// using System.Linq;
// using AppointmentApplication.Application.Features.Appointments.Dtos;
// using AppointmentApplication.Application.Features.Billings.Dtos;
// using AppointmentApplication.Application.Features.Billings.Mappers;
// using AppointmentApplication.Domain.Appointments;

// namespace AppointmentApplication.Application.Features.Appointments.Mappers
// {
//     public static class AppointmentMapper
//     {
//         public static AppointmentDto ToDto(this Appointment appointment)
//         {
//             // return new AppointmentDto(
//             //     appointment.Id,
//             //     appointment.PatientId,
//             //     appointment.DoctorId,
//             //     appointment.FacilityId,
//             //     appointment.ScheduledDate,
//             //     appointment.ScheduledTime,
//             //     appointment.DurationMinutes,
//             //     appointment.Status,
//             //     appointment.BookingDate,
//             //     appointment.CheckInTime,
//             //     appointment.CheckOutTime,
//             //     appointment.Notes,
//             //     appointment.CancellationReason,
//             //     new PatientDto(
//             //         appointment.Patient.Id,
//             //         appointment.Patient.FirstName,
//             //         appointment.Patient.LastName,
//             //         appointment.Patient.Email,
//             //         appointment.Patient.PhoneNumber
//             //     ),
//             //     new DoctorDto(
//             //         appointment.Doctor.Id,
//             //         appointment.Doctor.FirstName,
//             //         appointment.Doctor.LastName,
//             //         appointment.Doctor.Specialization.ToString()
//             //     ),
//             //     new FacilityDto(
//             //         appointment.Facility.Id,
//             //         appointment.Facility.Name,
//             //         appointment.Facility.City
//             //     ),
//             //     appointment.Billing?.ToDto(),
//             //     appointment.Prescriptions.Select(p => p.ToDto()).ToList()
//             // );
//         }
//     }
// }