using System.Collections.Generic;
using System.Linq;

using AppointmentApplication.Application.Features.Billings.Dtos;
using AppointmentApplication.Domain.Billings;
using AppointmentApplication.Domain.Billings.Enums;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Patients;

namespace AppointmentApplication.Application.Features.Billings.Mappers
{
    public static class BillingMapper
    {
        public static BillingDto? ToDto(this Billing billing)
        {
            if (billing == null)
            {
                return null;
            }

            // Map Patient - استخدام الاسم الكامل
            AppointmentApplication.Application.Features.Billings.Dtos.PatientInfoDto? patientDto = null;
            if (billing.Patient != null)
            {
                patientDto = new AppointmentApplication.Application.Features.Billings.Dtos.PatientInfoDto(
                    Id: billing.Patient.Id,
                    FullName: $"{billing.Patient.FirstName} {billing.Patient.LastName}".Trim(),
                    NationalID: billing.Patient.NationalID ?? "N/A"
                );
            }

            // Map Doctor - استخدام الاسم الكامل
            AppointmentApplication.Application.Features.Billings.Dtos.DoctorInfoDto? doctorDto = null;
            if (billing.Doctor != null)
            {
                doctorDto = new AppointmentApplication.Application.Features.Billings.Dtos.DoctorInfoDto(
                    Id: billing.Doctor.Id,
                    FullName: $"{billing.Doctor.FirstName} {billing.Doctor.LastName}".Trim(),
                    Specialization: billing.Doctor.Specialization.ToString()
                );
            }

            // Map Appointment - استخدام الاسم الكامل
            AppointmentApplication.Application.Features.Billings.Dtos.AppointmentInfoDto? appointmentDto = null;
            if (billing.Appointment != null)
            {
                appointmentDto = new AppointmentApplication.Application.Features.Billings.Dtos.AppointmentInfoDto(
                    Id: billing.Appointment.Id,
                    ScheduledDate: billing.Appointment.ScheduledDate,
                    ScheduledTime: billing.Appointment.ScheduledTime,
                    Status: billing.Appointment.Status.ToString()
                );
            }

            return new BillingDto(
                Id: billing.Id,
                AppointmentId: billing.AppointmentId,
                PatientId: billing.PatientId,
                DoctorId: billing.DoctorId,
                DateIssued: billing.DateIssued,
                TotalAmount: billing.TotalAmount,
                Status: billing.Status,
                PaymentDate: billing.PaymentDate,
                PaidAmount: billing.PaidAmount,
                Patient: patientDto,
                Doctor: doctorDto,
                Appointment: appointmentDto
            );
        }

        public static List<BillingDto> ToDtos(this IEnumerable<Billing> billings)
        {
            return billings?
                .Where(b => b != null)
                .Select(ToDto)
                .Where(dto => dto != null)
                .ToList() ?? [];
        }
    }
}