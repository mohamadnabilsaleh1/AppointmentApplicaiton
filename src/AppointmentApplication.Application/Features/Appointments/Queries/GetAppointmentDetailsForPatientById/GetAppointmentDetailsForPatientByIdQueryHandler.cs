// AppointmentApplication.Application/Features/Appointments/Queries/GetAppointmentDetailsForPatientById/GetAppointmentDetailsForPatientByIdQueryHandler.cs
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Appointments.Dtos;
using AppointmentApplication.Application.Features.Appointments.Errors;
using AppointmentApplication.Application.Features.Patients.Errors;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Application.Shared.Utilities;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Appointments.Enums;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Appointments.Queries.GetAppointmentDetailsForPatientById
{
    public class GetAppointmentDetailsForPatientByIdQueryHandler
        : IRequestHandler<GetAppointmentDetailsForPatientByIdQuery, Result<AppointmentDetailsDto>>
    {
        private readonly IAppDbContext _context;

        public GetAppointmentDetailsForPatientByIdQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<AppointmentDetailsDto>> Handle(
            GetAppointmentDetailsForPatientByIdQuery request,
            CancellationToken cancellationToken)
        {
            // Find patient by user ID
            var patient = await _context.Patients
                .Include(p => p.User)
                .Include(p => p.Allergies)
                .Include(p => p.ChronicDiseases)
                .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

            if (patient is null)
            {
                return ApplicationPatientErrors.PatientNotFound(request.UserId);
            }

            // Build base query for appointment with all includes
            var appointmentQuery = _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Facility)
                    .ThenInclude(f => f.Address)
                .Include(a => a.Billing)
                .Include(a => a.Prescriptions)
                .Where(a => a.Id == request.AppointmentId && a.PatientId == patient.Id);

            // Execute query
            var appointment = await appointmentQuery.FirstOrDefaultAsync(cancellationToken);

            if (appointment is null)
            {
                return ApplicationAppointmentErrors.AppointmentNotFound(request.AppointmentId);
            }

            // Map to detailed DTO based on appointment status
            var appointmentDetails = appointment.Status == AppointmentStatus.Completed
                ? MapToCompletedAppointmentDetails(appointment)
                : MapToBasicAppointmentDetails(appointment);

            return appointmentDetails;
        }

        private AppointmentDetailsDto MapToCompletedAppointmentDetails(Appointment appointment)
        {
            return new AppointmentDetailsDto(
                Id: appointment.Id,
                ScheduledDate: appointment.ScheduledDate,
                ScheduledTime: appointment.ScheduledTime,
                DurationMinutes: appointment.DurationMinutes,
                Status: appointment.Status,
                BookingDate: appointment.BookingDate,
                CheckInTime: appointment.CheckInTime,
                CheckOutTime: appointment.CheckOutTime,
                Notes: appointment.Notes,
                CancellationReason: appointment.CancellationReason,

                Patient: MapToPatientDetails(appointment.Patient),
                Doctor: MapToDoctorDetails(appointment.Doctor),
                Facility: MapToFacilityDetails(appointment.Facility),

                // Include billing and prescriptions for completed appointments
                Billing: appointment.Billing != null ? MapToBillingDetails(appointment.Billing) : null,
                Prescriptions: appointment.Prescriptions?.Select(MapToPrescriptionDetails).ToList()
            );
        }

        private AppointmentDetailsDto MapToBasicAppointmentDetails(Appointment appointment)
        {
            return new AppointmentDetailsDto(
                Id: appointment.Id,
                ScheduledDate: appointment.ScheduledDate,
                ScheduledTime: appointment.ScheduledTime,
                DurationMinutes: appointment.DurationMinutes,
                Status: appointment.Status,
                BookingDate: appointment.BookingDate,
                CheckInTime: appointment.CheckInTime,
                CheckOutTime: appointment.CheckOutTime,
                Notes: appointment.Notes,
                CancellationReason: appointment.CancellationReason,
                Patient: MapToPatientDetails(appointment.Patient),
                Doctor: MapToDoctorDetails(appointment.Doctor),
                Facility: MapToFacilityDetails(appointment.Facility),
                Billing: null,
                Prescriptions: null
            );
        }

        private PatientDetailsDto MapToPatientDetails(Domain.Patients.Patient patient)
        {
            return new PatientDetailsDto(
                Id: patient.Id,
                FullName: $"{patient.FirstName} {patient.LastName}",
                NationalID: patient.NationalID,
                Gender: patient.Gender.ToString(),
                Age: AgeCalculator.CalculateAge(patient.DateOfBirth));
        }

        private DoctorDetailsDto MapToDoctorDetails(Domain.Doctors.Doctor doctor)
        {
            return new DoctorDetailsDto(
                Id: doctor.Id,
                FullName: $"{doctor.FirstName} {doctor.LastName}",
                Gender: doctor.Gender.ToString(),
                Age: AgeCalculator.CalculateAge(doctor.DateOfBirth), LicenseNumber: doctor.LicenseNumber,
                Specialization: doctor.Specialization.ToString()
            );
        }

        private FacilityDetailsDto MapToFacilityDetails(Domain.HealthcareFacilities.HealthCareFacility facility)
        {
            return new FacilityDetailsDto(
                Id: facility.Id,
                Name: facility.Name,
                Type: facility.Type.ToString(),
                Address: new AddressDto(
                    facility.Address.Street,
                    facility.Address.City,
                    facility.Address.State,
                    facility.Address.Country,
                    facility.Address.ZipCode
                ),
                GPSLatitude: facility.GPSLatitude,
                GPSLongitude: facility.GPSLongitude
                            );
        }

        private BillingDetailsDto MapToBillingDetails(Domain.Billings.Billing billing)
        {
            return new BillingDetailsDto(
                Id: billing.Id,
                TotalAmount: billing.TotalAmount,
                Status: billing.Status.ToString(),
                DateIssued: billing.DateIssued,
                PaymentDate: billing.PaymentDate,
                PaidAmount: billing.PaidAmount
            );
        }

        private PrescriptionDetailsDto MapToPrescriptionDetails(Domain.Prescriptions.Prescription prescription)
        {
            return new PrescriptionDetailsDto(
                Id: prescription.Id,
                DateIssued: prescription.DateIssued,
                MedicationList: prescription.MedicationList,
                DosageInstructions: prescription.DosageInstructions
            );
        }
    }
}