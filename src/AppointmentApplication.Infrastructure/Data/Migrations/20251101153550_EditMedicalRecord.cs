using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditMedicalRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Appointments_AppointmentID",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Doctors_DoctorID",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_HealthCareFacilities_FacilityID",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Patients_PatientID",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Appointments_AppointmentID",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Doctors_IssuedByDoctorID",
                table: "Prescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_billings",
                table: "billings");

            migrationBuilder.DropIndex(
                name: "IX_billing_payments_BillingID",
                table: "billing_payments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "RecordType",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "BillingPaymentID",
                table: "billings");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "billings");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "billing_payments");

            migrationBuilder.DropColumn(
                name: "TransactionReference",
                table: "billing_payments");

            migrationBuilder.RenameTable(
                name: "billings",
                newName: "Billings");

            migrationBuilder.RenameColumn(
                name: "AppointmentID",
                table: "Prescriptions",
                newName: "AppointmentId");

            migrationBuilder.RenameColumn(
                name: "IssuedByDoctorID",
                table: "Prescriptions",
                newName: "DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Prescriptions_AppointmentID",
                table: "Prescriptions",
                newName: "IX_Prescriptions_AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Prescriptions_IssuedByDoctorID",
                table: "Prescriptions",
                newName: "IX_Prescriptions_DoctorId");

            migrationBuilder.RenameColumn(
                name: "PatientID",
                table: "MedicalRecords",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "FacilityID",
                table: "MedicalRecords",
                newName: "FacilityId");

            migrationBuilder.RenameColumn(
                name: "DoctorID",
                table: "MedicalRecords",
                newName: "DoctorId");

            migrationBuilder.RenameColumn(
                name: "AppointmentID",
                table: "MedicalRecords",
                newName: "AppointmentId");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "MedicalRecords",
                newName: "TreatmentNotes");

            migrationBuilder.RenameColumn(
                name: "Details",
                table: "MedicalRecords",
                newName: "FollowUpInstructions");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "MedicalRecords",
                newName: "RecordDate");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_PatientID",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_FacilityID",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_DoctorID",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_AppointmentID",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_AppointmentId");

            migrationBuilder.RenameColumn(
                name: "PatientID",
                table: "Billings",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "DoctorID",
                table: "Billings",
                newName: "DoctorId");

            migrationBuilder.RenameColumn(
                name: "AppointmentID",
                table: "Billings",
                newName: "AppointmentId");

            migrationBuilder.RenameIndex(
                name: "IX_billings_PatientID",
                table: "Billings",
                newName: "IX_Billings_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_billings_DoctorID",
                table: "Billings",
                newName: "IX_Billings_DoctorId");

            migrationBuilder.RenameIndex(
                name: "IX_billings_AppointmentID",
                table: "Billings",
                newName: "IX_Billings_AppointmentId");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppointmentId",
                table: "MedicalRecords",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "MedicalRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Billings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "Billings",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Billings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Appointments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CancellationReason",
                table: "Appointments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Billings",
                table: "Billings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_billing_payments_BillingID",
                table: "billing_payments",
                column: "BillingID");

            migrationBuilder.AddForeignKey(
                name: "FK_billing_payments_Billings_BillingID",
                table: "billing_payments",
                column: "BillingID",
                principalTable: "Billings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Appointments_AppointmentId",
                table: "Billings",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Doctors_DoctorId",
                table: "Billings",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_Patients_PatientId",
                table: "Billings",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Appointments_AppointmentId",
                table: "MedicalRecords",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Doctors_DoctorId",
                table: "MedicalRecords",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_HealthCareFacilities_FacilityId",
                table: "MedicalRecords",
                column: "FacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Patients_PatientId",
                table: "MedicalRecords",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Appointments_AppointmentId",
                table: "Prescriptions",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Doctors_DoctorId",
                table: "Prescriptions",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_billing_payments_Billings_BillingID",
                table: "billing_payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Appointments_AppointmentId",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Doctors_DoctorId",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_Patients_PatientId",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Appointments_AppointmentId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Doctors_DoctorId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_HealthCareFacilities_FacilityId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_Patients_PatientId",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Appointments_AppointmentId",
                table: "Prescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Doctors_DoctorId",
                table: "Prescriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Billings",
                table: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_billing_payments_BillingID",
                table: "billing_payments");

            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "MedicalRecords");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Billings");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Billings");

            migrationBuilder.RenameTable(
                name: "Billings",
                newName: "billings");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "Prescriptions",
                newName: "AppointmentID");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Prescriptions",
                newName: "IssuedByDoctorID");

            migrationBuilder.RenameIndex(
                name: "IX_Prescriptions_AppointmentId",
                table: "Prescriptions",
                newName: "IX_Prescriptions_AppointmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Prescriptions_DoctorId",
                table: "Prescriptions",
                newName: "IX_Prescriptions_IssuedByDoctorID");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "MedicalRecords",
                newName: "PatientID");

            migrationBuilder.RenameColumn(
                name: "FacilityId",
                table: "MedicalRecords",
                newName: "FacilityID");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "MedicalRecords",
                newName: "DoctorID");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "MedicalRecords",
                newName: "AppointmentID");

            migrationBuilder.RenameColumn(
                name: "TreatmentNotes",
                table: "MedicalRecords",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "RecordDate",
                table: "MedicalRecords",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "FollowUpInstructions",
                table: "MedicalRecords",
                newName: "Details");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_PatientId",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_PatientID");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_FacilityId",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_FacilityID");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_DoctorId",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_DoctorID");

            migrationBuilder.RenameIndex(
                name: "IX_MedicalRecords_AppointmentId",
                table: "MedicalRecords",
                newName: "IX_MedicalRecords_AppointmentID");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "billings",
                newName: "PatientID");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "billings",
                newName: "DoctorID");

            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "billings",
                newName: "AppointmentID");

            migrationBuilder.RenameIndex(
                name: "IX_Billings_PatientId",
                table: "billings",
                newName: "IX_billings_PatientID");

            migrationBuilder.RenameIndex(
                name: "IX_Billings_DoctorId",
                table: "billings",
                newName: "IX_billings_DoctorID");

            migrationBuilder.RenameIndex(
                name: "IX_Billings_AppointmentId",
                table: "billings",
                newName: "IX_billings_AppointmentID");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Prescriptions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppointmentID",
                table: "MedicalRecords",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "RecordType",
                table: "MedicalRecords",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MedicalRecords",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Active");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "billings",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<Guid>(
                name: "BillingPaymentID",
                table: "billings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "billings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "billing_payments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Completed");

            migrationBuilder.AddColumn<string>(
                name: "TransactionReference",
                table: "billing_payments",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CancellationReason",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldDefaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_billings",
                table: "billings",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Appointments_AppointmentID",
                table: "MedicalRecords",
                column: "AppointmentID",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Doctors_DoctorID",
                table: "MedicalRecords",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_HealthCareFacilities_FacilityID",
                table: "MedicalRecords",
                column: "FacilityID",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_Patients_PatientID",
                table: "MedicalRecords",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Appointments_AppointmentID",
                table: "Prescriptions",
                column: "AppointmentID",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Doctors_IssuedByDoctorID",
                table: "Prescriptions",
                column: "IssuedByDoctorID",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
