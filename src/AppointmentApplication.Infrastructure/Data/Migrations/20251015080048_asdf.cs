using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class asdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillingPayments_Billings_BillingID",
                table: "BillingPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Appointments_AppointmentID",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Doctors_DoctorID",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_HealthCareFacilities_FacilityID",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Patients_PatientID",
                table: "Billings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Billings",
                table: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_Billings_AppointmentID",
                table: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_Billings_FacilityID",
                table: "Billings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BillingPayments",
                table: "BillingPayments");

            migrationBuilder.DropIndex(
                name: "IX_BillingPayments_BillingID",
                table: "BillingPayments");

            migrationBuilder.DropColumn(
                name: "FacilityID",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "RescheduledFrom",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "Billings",
                newName: "billings");

            migrationBuilder.RenameTable(
                name: "BillingPayments",
                newName: "billing_payments");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "users",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "ScheduleExceptionDoctors",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "ScheduleDoctors",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "Reviews",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "Prescriptions",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "Phones",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "PatientUploads",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "Patients",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "MedicalRecords",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "MedicalRecordAttachments",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "HealthcareFacilitySchedules",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "HealthcareFacilityScheduleExceptions",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "HealthCareFacilities",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "FacilityUploads",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "Emails",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "DoctorTreatmentCapacities",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "Doctors",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "Departments",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "billings",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameIndex(
                name: "IX_Billings_PatientID",
                table: "billings",
                newName: "IX_billings_PatientID");

            migrationBuilder.RenameIndex(
                name: "IX_Billings_DoctorID",
                table: "billings",
                newName: "IX_billings_DoctorID");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "Appointments",
                newName: "UpdatedAtUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtdUtc",
                table: "billing_payments",
                newName: "UpdatedAtUtc");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "PatientChronicDiseases",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "PatientChronicDiseases",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "PatientAllergies",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "PatientAllergies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "billings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "BillingPaymentID",
                table: "billings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BillingID",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "TransactionReference",
                table: "billing_payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "billing_payments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddPrimaryKey(
                name: "PK_billings",
                table: "billings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_billing_payments",
                table: "billing_payments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_billings_AppointmentID",
                table: "billings",
                column: "AppointmentID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_billing_payments_BillingID",
                table: "billing_payments",
                column: "BillingID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_billing_payments_billings_BillingID",
                table: "billing_payments",
                column: "BillingID",
                principalTable: "billings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_billings_Appointments_AppointmentID",
                table: "billings",
                column: "AppointmentID",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_billings_Doctors_DoctorID",
                table: "billings",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_billings_Patients_PatientID",
                table: "billings",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_billing_payments_billings_BillingID",
                table: "billing_payments");

            migrationBuilder.DropForeignKey(
                name: "FK_billings_Appointments_AppointmentID",
                table: "billings");

            migrationBuilder.DropForeignKey(
                name: "FK_billings_Doctors_DoctorID",
                table: "billings");

            migrationBuilder.DropForeignKey(
                name: "FK_billings_Patients_PatientID",
                table: "billings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_billings",
                table: "billings");

            migrationBuilder.DropIndex(
                name: "IX_billings_AppointmentID",
                table: "billings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_billing_payments",
                table: "billing_payments");

            migrationBuilder.DropIndex(
                name: "IX_billing_payments_BillingID",
                table: "billing_payments");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "PatientChronicDiseases");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "PatientChronicDiseases");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "PatientAllergies");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "PatientAllergies");

            migrationBuilder.DropColumn(
                name: "BillingPaymentID",
                table: "billings");

            migrationBuilder.DropColumn(
                name: "BillingID",
                table: "Appointments");

            migrationBuilder.RenameTable(
                name: "billings",
                newName: "Billings");

            migrationBuilder.RenameTable(
                name: "billing_payments",
                newName: "BillingPayments");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "users",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "ScheduleExceptionDoctors",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "ScheduleDoctors",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "Reviews",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "Prescriptions",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "Phones",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "PatientUploads",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "Patients",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "MedicalRecords",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "MedicalRecordAttachments",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "HealthcareFacilitySchedules",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "HealthcareFacilityScheduleExceptions",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "HealthCareFacilities",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "FacilityUploads",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "Emails",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "DoctorTreatmentCapacities",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "Doctors",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "Departments",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "Billings",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameIndex(
                name: "IX_billings_PatientID",
                table: "Billings",
                newName: "IX_Billings_PatientID");

            migrationBuilder.RenameIndex(
                name: "IX_billings_DoctorID",
                table: "Billings",
                newName: "IX_Billings_DoctorID");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "Appointments",
                newName: "UpdatedAtdUtc");

            migrationBuilder.RenameColumn(
                name: "UpdatedAtUtc",
                table: "BillingPayments",
                newName: "UpdatedAtdUtc");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Billings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<Guid>(
                name: "FacilityID",
                table: "Billings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RescheduledFrom",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionReference",
                table: "BillingPayments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "BillingPayments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Billings",
                table: "Billings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BillingPayments",
                table: "BillingPayments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_AppointmentID",
                table: "Billings",
                column: "AppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Billings_FacilityID",
                table: "Billings",
                column: "FacilityID");

            migrationBuilder.CreateIndex(
                name: "IX_BillingPayments_BillingID",
                table: "BillingPayments",
                column: "BillingID");

            migrationBuilder.AddForeignKey(
                name: "FK_BillingPayments_Billings_BillingID",
                table: "BillingPayments",
                column: "BillingID",
                principalTable: "Billings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Appointments_AppointmentID",
                table: "Billings",
                column: "AppointmentID",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Doctors_DoctorID",
                table: "Billings",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_HealthCareFacilities_FacilityID",
                table: "Billings",
                column: "FacilityID",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Patients_PatientID",
                table: "Billings",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
