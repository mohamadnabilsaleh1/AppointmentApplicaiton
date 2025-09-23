using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Appointments;
using AppointmentApplication.Domain.Billings;
using AppointmentApplication.Domain.Billings.BillingPayments;
using AppointmentApplication.Domain.DoctorDepartments;
using AppointmentApplication.Domain.DoctorFacilities;
using AppointmentApplication.Domain.Doctors;
using AppointmentApplication.Domain.Doctors.DoctorsTreatmentCapabilities;
using AppointmentApplication.Domain.Doctors.ScheduleExceptions;
using AppointmentApplication.Domain.Doctors.Schedules;
using AppointmentApplication.Domain.Doctors.Specializations;
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
using AppointmentApplication.Domain.Patients.PatientAllergies;
using AppointmentApplication.Domain.Patients.PatientChronicDiseases;
using AppointmentApplication.Domain.Prescriptions;
using AppointmentApplication.Domain.Reviews;
using AppointmentApplication.Domain.Shared;
using AppointmentApplication.Domain.Shared.Phone;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AppointmentApplication.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Healthcare Facility Entities
    public DbSet<HealthCareFacility> HealthcareFacilities { get; set; }
    public DbSet<ScheduleHealthcareFacility> ScheduleHealthcareFacilities { get; set; }
    public DbSet<ScheduleExceptionHealthcareFacility> ScheduleExceptionHealthcareFacilities { get; set; }
    public DbSet<Department> Departments { get; set; }

    // Contact Information
    public DbSet<Email> Emails { get; set; }
    public DbSet<Phone> Phones { get; set; }

    // Medical Entities
    public DbSet<Specialization> Specializations { get; set; }
    public DbSet<Allergy> Allergies { get; set; }
    public DbSet<PatientChronicDisease> ChronicDiseases { get; set; }

    // Doctor Entities
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<DoctorFacility> DoctorFacilities { get; set; }
    public DbSet<DoctorDepartment> DoctorDepartments { get; set; }
    public DbSet<ScheduleDoctor> ScheduleDoctors { get; set; }
    public DbSet<ScheduleExceptionDoctor> ScheduleExceptionDoctors { get; set; }
    public DbSet<DoctorTreatmentCapacity> DoctorTreatmentCapacities { get; set; }

    // Patient Entities
    public DbSet<Patient> Patients { get; set; }
    public DbSet<PatientAllergy> PatientAllergies { get; set; }
    public DbSet<PatientChronicDisease> PatientChronicDiseases { get; set; }

    // Medical Records
    public DbSet<MedicalRecord> MedicalRecords { get; set; }
    public DbSet<MedicalRecordAttachment> MedicalRecordAttachments { get; set; }

    // Appointments & Billing
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Billing> Billings { get; set; }
    public DbSet<BillingPayment> BillingPayments { get; set; }

    // Reviews & Uploads
    public DbSet<Review> Reviews { get; set; }
    public DbSet<PatientUpload> PatientUploads { get; set; }
    public DbSet<FacilityUpload> FacilityUploads { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateAuditableEntities();
        return base.SaveChanges();
    }

    private void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry> entries = ChangeTracker.Entries()
                .Where(e => e.Entity is AuditableEntity &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (EntityEntry? entityEntry in entries)
        {
            var auditableEntity = (AuditableEntity)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added)
            {
                auditableEntity.CreatedAtUtc = DateTime.UtcNow;
            }
            else
            {
                auditableEntity.UpdatedAtdUtc = DateTime.UtcNow;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

    }
}
