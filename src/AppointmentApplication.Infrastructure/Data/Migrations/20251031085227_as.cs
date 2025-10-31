using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class @as : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_HealthCareFacilities_FacilityID",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientID",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "PatientID",
                table: "Appointments",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "FacilityID",
                table: "Appointments",
                newName: "FacilityId");

            migrationBuilder.RenameColumn(
                name: "DoctorID",
                table: "Appointments",
                newName: "DoctorId");

            migrationBuilder.RenameColumn(
                name: "BillingID",
                table: "Appointments",
                newName: "BillingId");

            migrationBuilder.RenameColumn(
                name: "CancelledReason",
                table: "Appointments",
                newName: "CancellationReason");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_PatientID",
                table: "Appointments",
                newName: "IX_Appointments_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_FacilityID",
                table: "Appointments",
                newName: "IX_Appointments_FacilityId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_DoctorID",
                table: "Appointments",
                newName: "IX_Appointments_DoctorId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "billings",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Appointments",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ScheduledDate",
                table: "Appointments",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "BillingId",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInTime",
                table: "Appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_HealthCareFacilities_FacilityId",
                table: "Appointments",
                column: "FacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Doctors_DoctorId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_HealthCareFacilities_FacilityId",
                table: "Appointments");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Patients_PatientId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CheckInTime",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Appointments",
                newName: "PatientID");

            migrationBuilder.RenameColumn(
                name: "FacilityId",
                table: "Appointments",
                newName: "FacilityID");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Appointments",
                newName: "DoctorID");

            migrationBuilder.RenameColumn(
                name: "BillingId",
                table: "Appointments",
                newName: "BillingID");

            migrationBuilder.RenameColumn(
                name: "CancellationReason",
                table: "Appointments",
                newName: "CancelledReason");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                newName: "IX_Appointments_PatientID");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_FacilityId",
                table: "Appointments",
                newName: "IX_Appointments_FacilityID");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                newName: "IX_Appointments_DoctorID");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "billings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Appointments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ScheduledDate",
                table: "Appointments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<Guid>(
                name: "BillingID",
                table: "Appointments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Doctors_DoctorID",
                table: "Appointments",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_HealthCareFacilities_FacilityID",
                table: "Appointments",
                column: "FacilityID",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Patients_PatientID",
                table: "Appointments",
                column: "PatientID",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
