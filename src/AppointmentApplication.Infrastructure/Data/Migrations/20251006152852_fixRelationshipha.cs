using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixRelationshipha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_HealthcareFacilities_FacilityID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_HealthcareFacilities_FacilityID",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_HealthcareFacilities_FacilityId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorDepartments_HealthcareFacilities_FacilityId",
                table: "DoctorDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_HealthcareFacilities_FacilityId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityUploads_HealthcareFacilities_FacilityId",
                table: "FacilityUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilities_users_UserId",
                table: "HealthcareFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthcareFacilities_FacilityId",
                table: "HealthcareFacilityScheduleExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthcareFacilities_FacilityId1",
                table: "HealthcareFacilityScheduleExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthcareFacilities_FacilityId",
                table: "HealthcareFacilitySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthcareFacilities_FacilityId1",
                table: "HealthcareFacilitySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_HealthcareFacilities_FacilityID",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_HealthcareFacilities_FacilityID",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthcareFacilities",
                table: "HealthcareFacilities");

            migrationBuilder.RenameTable(
                name: "HealthcareFacilities",
                newName: "HealthCareFacilities");

            migrationBuilder.RenameColumn(
                name: "FacilityId1",
                table: "HealthcareFacilitySchedules",
                newName: "HealthCareFacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthcareFacilitySchedules_FacilityId1",
                table: "HealthcareFacilitySchedules",
                newName: "IX_HealthcareFacilitySchedules_HealthCareFacilityId");

            migrationBuilder.RenameColumn(
                name: "FacilityId1",
                table: "HealthcareFacilityScheduleExceptions",
                newName: "HealthCareFacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthcareFacilityScheduleExceptions_FacilityId1",
                table: "HealthcareFacilityScheduleExceptions",
                newName: "IX_HealthcareFacilityScheduleExceptions_HealthCareFacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_HealthcareFacilities_UserId",
                table: "HealthCareFacilities",
                newName: "IX_HealthCareFacilities_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Specializations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "ScheduleExceptionDoctors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "ScheduleDoctors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Reviews",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Prescriptions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Phones",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "PatientUploads",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Patients",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "PatientChronicDisease",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "PatientAllergies",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "MedicalRecords",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "MedicalRecordAttachments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "HealthcareFacilitySchedules",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "HealthcareFacilityScheduleExceptions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "HealthCareFacilities",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "HealthCareFacilities",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "HealthCareFacilities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "FacilityUploads",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Emails",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "DoctorTreatmentCapacities",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Doctors",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "DoctorDepartments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Departments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "ChronicDisease",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Billings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "BillingPayments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Appointments",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Allergies",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthCareFacilities",
                table: "HealthCareFacilities",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_HealthCareFacilities_UserId1",
                table: "HealthCareFacilities",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_HealthCareFacilities_FacilityID",
                table: "Appointments",
                column: "FacilityID",
                principalTable: "HealthCareFacilities",
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
                name: "FK_Departments_HealthCareFacilities_FacilityId",
                table: "Departments",
                column: "FacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorDepartments_HealthCareFacilities_FacilityId",
                table: "DoctorDepartments",
                column: "FacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_HealthCareFacilities_FacilityId",
                table: "Doctors",
                column: "FacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityUploads_HealthCareFacilities_FacilityId",
                table: "FacilityUploads",
                column: "FacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCareFacilities_users_UserId",
                table: "HealthCareFacilities",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCareFacilities_users_UserId1",
                table: "HealthCareFacilities",
                column: "UserId1",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthCareFacilities_FacilityId",
                table: "HealthcareFacilityScheduleExceptions",
                column: "FacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthCareFacilities_HealthCareFacilityId",
                table: "HealthcareFacilityScheduleExceptions",
                column: "HealthCareFacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthCareFacilities_FacilityId",
                table: "HealthcareFacilitySchedules",
                column: "FacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthCareFacilities_HealthCareFacilityId",
                table: "HealthcareFacilitySchedules",
                column: "HealthCareFacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_HealthCareFacilities_FacilityID",
                table: "MedicalRecords",
                column: "FacilityID",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_HealthCareFacilities_FacilityID",
                table: "Reviews",
                column: "FacilityID",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_HealthCareFacilities_FacilityID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Billings_HealthCareFacilities_FacilityID",
                table: "Billings");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_HealthCareFacilities_FacilityId",
                table: "Departments");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorDepartments_HealthCareFacilities_FacilityId",
                table: "DoctorDepartments");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_HealthCareFacilities_FacilityId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_FacilityUploads_HealthCareFacilities_FacilityId",
                table: "FacilityUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareFacilities_users_UserId",
                table: "HealthCareFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareFacilities_users_UserId1",
                table: "HealthCareFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthCareFacilities_FacilityId",
                table: "HealthcareFacilityScheduleExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthCareFacilities_HealthCareFacilityId",
                table: "HealthcareFacilityScheduleExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthCareFacilities_FacilityId",
                table: "HealthcareFacilitySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthCareFacilities_HealthCareFacilityId",
                table: "HealthcareFacilitySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalRecords_HealthCareFacilities_FacilityID",
                table: "MedicalRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_HealthCareFacilities_FacilityID",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HealthCareFacilities",
                table: "HealthCareFacilities");

            migrationBuilder.DropIndex(
                name: "IX_HealthCareFacilities_UserId1",
                table: "HealthCareFacilities");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "HealthCareFacilities");

            migrationBuilder.RenameTable(
                name: "HealthCareFacilities",
                newName: "HealthcareFacilities");

            migrationBuilder.RenameColumn(
                name: "HealthCareFacilityId",
                table: "HealthcareFacilitySchedules",
                newName: "FacilityId1");

            migrationBuilder.RenameIndex(
                name: "IX_HealthcareFacilitySchedules_HealthCareFacilityId",
                table: "HealthcareFacilitySchedules",
                newName: "IX_HealthcareFacilitySchedules_FacilityId1");

            migrationBuilder.RenameColumn(
                name: "HealthCareFacilityId",
                table: "HealthcareFacilityScheduleExceptions",
                newName: "FacilityId1");

            migrationBuilder.RenameIndex(
                name: "IX_HealthcareFacilityScheduleExceptions_HealthCareFacilityId",
                table: "HealthcareFacilityScheduleExceptions",
                newName: "IX_HealthcareFacilityScheduleExceptions_FacilityId1");

            migrationBuilder.RenameIndex(
                name: "IX_HealthCareFacilities_UserId",
                table: "HealthcareFacilities",
                newName: "IX_HealthcareFacilities_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Specializations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "ScheduleExceptionDoctors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "ScheduleDoctors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Prescriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Phones",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "PatientUploads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Patients",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "PatientChronicDisease",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "PatientAllergies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "MedicalRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "MedicalRecordAttachments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "HealthcareFacilitySchedules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "HealthcareFacilityScheduleExceptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "HealthcareFacilities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "HealthcareFacilities",
                type: "int",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "FacilityUploads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Emails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "DoctorTreatmentCapacities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Doctors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "DoctorDepartments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Departments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "ChronicDisease",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Billings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "BillingPayments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "Allergies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HealthcareFacilities",
                table: "HealthcareFacilities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_HealthcareFacilities_FacilityID",
                table: "Appointments",
                column: "FacilityID",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Billings_HealthcareFacilities_FacilityID",
                table: "Billings",
                column: "FacilityID",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_HealthcareFacilities_FacilityId",
                table: "Departments",
                column: "FacilityId",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorDepartments_HealthcareFacilities_FacilityId",
                table: "DoctorDepartments",
                column: "FacilityId",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_HealthcareFacilities_FacilityId",
                table: "Doctors",
                column: "FacilityId",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityUploads_HealthcareFacilities_FacilityId",
                table: "FacilityUploads",
                column: "FacilityId",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilities_users_UserId",
                table: "HealthcareFacilities",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthcareFacilities_FacilityId",
                table: "HealthcareFacilityScheduleExceptions",
                column: "FacilityId",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthcareFacilities_FacilityId1",
                table: "HealthcareFacilityScheduleExceptions",
                column: "FacilityId1",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthcareFacilities_FacilityId",
                table: "HealthcareFacilitySchedules",
                column: "FacilityId",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthcareFacilities_FacilityId1",
                table: "HealthcareFacilitySchedules",
                column: "FacilityId1",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalRecords_HealthcareFacilities_FacilityID",
                table: "MedicalRecords",
                column: "FacilityID",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_HealthcareFacilities_FacilityID",
                table: "Reviews",
                column: "FacilityID",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
