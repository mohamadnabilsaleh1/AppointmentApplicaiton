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

        // HealthcareFacility configuration
        modelBuilder.Entity<HealthCareFacility>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(100);
            entity.OwnsOne(e => e.Address, address =>
             {
                 address.Property(a => a.Street).IsRequired().HasMaxLength(200);
                 address.Property(a => a.City).IsRequired().HasMaxLength(100);
                 address.Property(a => a.State).IsRequired().HasMaxLength(100);
                 address.Property(a => a.Country).IsRequired().HasMaxLength(100);
                 address.Property(a => a.ZipCode).IsRequired().HasMaxLength(20);
             });
            entity.Property(e => e.GPSLatitude).HasColumnType("decimal(9,6)");
            entity.Property(e => e.GPSLongitude).HasColumnType("decimal(9,6)");
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);
        });

        // ScheduleHealthcareFacility configuration
        modelBuilder.Entity<ScheduleHealthcareFacility>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DayOfWeek).IsRequired();
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.EndTime).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsAvailable).IsRequired();
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne<HealthCareFacility>()
                .WithMany(f => f.Schedules)
                .HasForeignKey(s => s.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ScheduleExceptionHealthcareFacility configuration
        modelBuilder.Entity<ScheduleExceptionHealthcareFacility>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.DayOfWeek).IsRequired();
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.EndTime).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

 entity.HasOne<HealthCareFacility>()
          .WithMany(f => f.ScheduleExceptions)
          .HasForeignKey(s => s.FacilityId)
          .OnDelete(DeleteBehavior.Restrict);
        });

        // Department configuration
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(d => d.Facility)        // استخدم property Facility من Department
                  .WithMany(f => f.Departments)   // استخدم collection Departments من HealthCareFacility
                  .HasForeignKey(d => d.FacilityId)
                  .OnDelete(DeleteBehavior.Restrict); // مهم لتجنب Multiple Restrict Paths

        });

        // Email configuration
        modelBuilder.Entity<Email>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.OwnerType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.OwnerId).IsRequired();
            entity.Property(e => e.EmailAddress).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Label).HasMaxLength(50);
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasIndex(e => new { e.OwnerType, e.OwnerId });
        });

        // Phone configuration
        modelBuilder.Entity<Phone>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.OwnerType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.OwnerId).IsRequired();
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Label).HasMaxLength(50);
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasIndex(e => new { e.OwnerType, e.OwnerId });
        });

        // Specialization configuration
        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description);
        });

        // Allergy configuration
        modelBuilder.Entity<Allergy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        });

        // ChronicDisease configuration
        // modelBuilder.Entity<ChronicDisease>(entity =>
        // {
        //     entity.HasKey(e => e.Id);
        //     entity.Property(e => e.Id).ValueGeneratedNever();
        //     entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        // });

        // Doctor configuration
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Gender).IsRequired().HasMaxLength(10);
            entity.Property(e => e.DateOfBirth).IsRequired();
            entity.Property(e => e.LicenseNumber).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // DoctorFacility configuration
        modelBuilder.Entity<DoctorFacility>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(df => df.Doctor)
                .WithMany(d => d.Facilities)
                .HasForeignKey(df => df.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(df => df.Facility)
                .WithMany()
                .HasForeignKey(df => df.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.DoctorId, e.FacilityId }).IsUnique();
        });

        // DoctorDepartment configuration
        modelBuilder.Entity<DoctorDepartment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(dd => dd.Doctor)
                .WithMany(d => d.Departments)
                .HasForeignKey(dd => dd.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(dd => dd.Department)
                .WithMany(d => d.DoctorDepartments)
                .HasForeignKey(dd => dd.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(dd => dd.Facility)
                .WithMany()
                .HasForeignKey(dd => dd.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.DoctorId, e.DepartmentId, e.FacilityId }).IsUnique();
        });

        // ScheduleDoctor configuration
        modelBuilder.Entity<ScheduleDoctor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DayOfWeek).IsRequired();
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.EndTime).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsAvailable).IsRequired();
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne<Doctor>()
                .WithMany(d => d.Schedules)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ScheduleExceptionDoctor configuration
        modelBuilder.Entity<ScheduleExceptionDoctor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.DayOfWeek).IsRequired();
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.EndTime).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne<Doctor>()
                .WithMany(d => d.ScheduleExceptions)
                .HasForeignKey(s => s.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // DoctorTreatmentCapacity configuration
        modelBuilder.Entity<DoctorTreatmentCapacity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.MaxPatientsPerDay).IsRequired().HasDefaultValue(10);
            entity.Property(e => e.SessionDurationMinutes).IsRequired().HasDefaultValue(30);
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(dtc => dtc.Doctor)
                .WithOne()
                .HasForeignKey<DoctorTreatmentCapacity>(dtc => dtc.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.DoctorId).IsUnique();
        });

        // Patient configuration
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.NationalID).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Gender).IsRequired().HasMaxLength(10);
            entity.Property(e => e.DateOfBirth).IsRequired();
            entity.Property(e => e.BloodType).HasMaxLength(5);
            entity.Property(e => e.IsActive).IsRequired();
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasIndex(e => e.NationalID).IsUnique();
        });

        // PatientAllergy configuration
        modelBuilder.Entity<PatientAllergy>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(pa => pa.Patient)
                .WithMany(p => p.Allergies)
                .HasForeignKey(pa => pa.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // entity.HasOne(pa => pa.Allergy)
            //     .WithMany(a => a.PatientAllergies)
            //     .HasForeignKey(pa => pa.AllergyId)
            //     .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.PatientId, e.AllergyId }).IsUnique();
        });

        // PatientChronicDisease configuration
        modelBuilder.Entity<PatientChronicDisease>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(pcd => pcd.Patient)
                .WithMany(p => p.ChronicDiseases)
                .HasForeignKey(pcd => pcd.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            // entity.HasOne(pcd => pcd.ChronicDisease)
            //     .WithMany(cd => cd.PatientChronicDiseases)
            //     .HasForeignKey(pcd => pcd.ChronicDiseaseId)
            //     .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.PatientId, e.ChronicDiseaseId }).IsUnique();
        });

        // MedicalRecord configuration
        modelBuilder.Entity<MedicalRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.RecordType).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DateCreated).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Active");
            entity.Property(e => e.Details).IsRequired();
            entity.Property(e => e.Notes);
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(mr => mr.Patient)
                .WithMany(p => p.MedicalRecords)
                .HasForeignKey(mr => mr.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(mr => mr.Facility)
                .WithMany()
                .HasForeignKey(mr => mr.FacilityID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(mr => mr.Doctor)
                .WithMany()
                .HasForeignKey(mr => mr.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(mr => mr.Appointment)
                .WithMany()
                .HasForeignKey(mr => mr.AppointmentID)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // MedicalRecordAttachment configuration
        modelBuilder.Entity<MedicalRecordAttachment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FileType).IsRequired().HasMaxLength(20).HasDefaultValue("Image");
            entity.Property(e => e.FileURL).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Description);
            entity.Property(e => e.Visibility).IsRequired().HasMaxLength(20).HasDefaultValue("Private");
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.UploadedAt).IsRequired();
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(mra => mra.MedicalRecord)
                .WithMany(mr => mr.Attachments)
                .HasForeignKey(mra => mra.MedicalRecordID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Appointment configuration
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ScheduledDate).IsRequired();
            entity.Property(e => e.ScheduledTime).IsRequired();
            entity.Property(e => e.DurationMinutes).IsRequired().HasDefaultValue(30);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.BookingDate).IsRequired();
            entity.Property(e => e.Notes);
            entity.Property(e => e.CancelledReason);
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            // entity.HasOne(a => a.Doctor)
            //     .WithMany(d => d.Appointments)
            //     .HasForeignKey(a => a.DoctorID)
            //     .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Facility)
                .WithMany()
                .HasForeignKey(a => a.FacilityID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Department)
                .WithMany()
                .HasForeignKey(a => a.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

            // entity.HasOne(a => a.RescheduledFromAppointment)
            //     .WithMany()
            //     .HasForeignKey(a => a.RescheduledFrom)
            //     .OnDelete(DeleteBehavior.SetNull);
        });

        // Prescription configuration
        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateIssued).IsRequired();
            entity.Property(e => e.MedicationList).IsRequired();
            entity.Property(e => e.DosageInstructions).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(p => p.Appointment)
                .WithMany(a => a.Prescriptions)
                .HasForeignKey(p => p.AppointmentID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Doctor)
                .WithMany()
                .HasForeignKey(p => p.IssuedByDoctorID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Billing configuration
        modelBuilder.Entity<Billing>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateIssued).IsRequired();
            entity.Property(e => e.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20).HasDefaultValue("Pending");
            entity.Property(e => e.Notes);
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(b => b.Patient)
                .WithMany()
                .HasForeignKey(b => b.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Facility)
                .WithMany()
                .HasForeignKey(b => b.FacilityID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Appointment)
                .WithMany(a => a.Billings)
                .HasForeignKey(b => b.AppointmentID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(b => b.Doctor)
                .WithMany()
                .HasForeignKey(b => b.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // BillingPayment configuration
        modelBuilder.Entity<BillingPayment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(20);
            entity.Property(e => e.PaidAmount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.PaymentDate).IsRequired();
            entity.Property(e => e.TransactionReference);
            entity.Property(e => e.PaymentStatus).IsRequired().HasMaxLength(20).HasDefaultValue("Completed");
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(bp => bp.Billing)
                .WithMany(b => b.Payments)
                .HasForeignKey(bp => bp.BillingID)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Review configuration
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Rating).IsRequired();
            entity.Property(e => e.Comment);
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(r => r.Patient)
                .WithMany()
                .HasForeignKey(r => r.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Facility)
                .WithMany()
                .HasForeignKey(r => r.FacilityID)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Doctor)
                .WithMany()
                .HasForeignKey(r => r.DoctorID)
                .OnDelete(DeleteBehavior.Restrict);

            // // Ensure either FacilityID or DoctorID is provided, but not both
            // entity.HasCheckConstraint("CK_Review_FacilityOrDoctor", 
            //     "(\"FacilityID\" IS NOT NULL AND \"DoctorID\" IS NULL) OR (\"FacilityID\" IS NULL AND \"DoctorID\" IS NOT NULL)");
        });

        // PatientUpload configuration
        modelBuilder.Entity<PatientUpload>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FileType).IsRequired().HasMaxLength(20).HasDefaultValue("Image");
            entity.Property(e => e.FileURL).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Description);
            entity.Property(e => e.Visibility).IsRequired().HasMaxLength(20).HasDefaultValue("Public");
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.UploadedAt).IsRequired();
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(pu => pu.Patient)
                .WithMany()
                .HasForeignKey(pu => pu.PatientId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // FacilityUploadMedia configuration
        modelBuilder.Entity<FacilityUpload>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FileType).IsRequired().HasMaxLength(20).HasDefaultValue("Image");
            entity.Property(e => e.FileURL).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Description);
            entity.Property(e => e.Visibility).IsRequired().HasMaxLength(20).HasDefaultValue("Public");
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(e => e.UploadedAt).IsRequired();
            entity.Property(e => e.CreatedAtUtc).IsRequired();
            entity.Property(e => e.UpdatedAtdUtc);

            entity.HasOne(fum => fum.Facility)
                .WithMany()
                .HasForeignKey(fum => fum.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure global query filters
        modelBuilder.Entity<HealthCareFacility>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Department>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Doctor>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Patient>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<MedicalRecord>().HasQueryFilter(e => e.Status != "Deleted");
        modelBuilder.Entity<MedicalRecordAttachment>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<PatientUpload>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<FacilityUpload>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<DoctorFacility>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<DoctorDepartment>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<DoctorTreatmentCapacity>().HasQueryFilter(e => e.IsActive);
    }
}
