using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Billings;
using AppointmentApplication.Domain.Billings.BillingPayments;
using AppointmentApplication.Domain.DoctorDepartments;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;
using AppointmentApplication.Domain.Doctors.ScheduleExceptions;
using AppointmentApplication.Domain.Doctors.Schedules;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.HealthcareFacilities.Departments;
using AppointmentApplication.Domain.HealthcareFacilities.ScheduleExceptions;
using AppointmentApplication.Domain.HealthcareFacilities.Schedules;
using AppointmentApplication.Domain.MediaUploads;
using AppointmentApplication.Domain.MedicalRecordAttachments;
using AppointmentApplication.Domain.MedicalRecords;
using AppointmentApplication.Domain.Patients;
using AppointmentApplication.Domain.Patients.Allergies;
using AppointmentApplication.Domain.Patients.ChronicDiseases;
using AppointmentApplication.Domain.Prescriptions;
using AppointmentApplication.Domain.Reviews;
using AppointmentApplication.Domain.Shared;
using AppointmentApplication.Domain.Shared.Phone;
using AppointmentApplication.Domain.Users;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Shared.Interfaces;

public interface IAppDbContext
{
    // Healthcare Facility Entities
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<Permission> Permissions { get; set; }
    DbSet<RolePermission> RolePermissions { get; set; }
    DbSet<HealthCareFacility> HealthcareFacilities { get; set; }
    DbSet<HealthCareFacilitySchedule> HealthcareFacilitySchedules { get; set; }
    DbSet<HealthCareFacilityScheduleException> HealthcareFacilityScheduleExceptions { get; set; }
    DbSet<Department> Departments { get; set; }

    // Contact Information
    DbSet<Email> Emails { get; set; }
    DbSet<Phone> Phones { get; set; }

    // Medical Entities
    DbSet<Allergy> Allergies { get; set; }

    // Doctor Entities
    DbSet<Doctor> Doctors { get; set; }

    DbSet<DoctorDepartment> DoctorDepartments { get; set; }
    DbSet<DoctorSchedule> ScheduleDoctors { get; set; }
    DbSet<DoctorScheduleException> ScheduleExceptionDoctors { get; set; }
    DbSet<DoctorTreatmentCapacity> DoctorTreatmentCapacities { get; set; }

    // Patient Entities
    DbSet<Patient> Patients { get; set; }

    DbSet<ChronicDisease> ChronicDiseases { get; set; }

    // Medical Records
    DbSet<MedicalRecord> MedicalRecords { get; set; }
    DbSet<MedicalRecordAttachment> MedicalRecordAttachments { get; set; }

    // Appointments & Billing
    DbSet<Appointment> Appointments { get; set; }
    DbSet<Prescription> Prescriptions { get; set; }
    DbSet<Billing> Billings { get; set; }
    DbSet<BillingPayment> BillingPayments { get; set; }

    // Reviews & Uploads
    DbSet<Review> Reviews { get; set; }
    DbSet<PatientUpload> PatientUploads { get; set; }
    DbSet<FacilityUpload> FacilityUploads { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}
