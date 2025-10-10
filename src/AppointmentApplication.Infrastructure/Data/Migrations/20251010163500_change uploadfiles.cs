using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class changeuploadfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "PatientUploads");

            migrationBuilder.DropColumn(
                name: "UploadedAt",
                table: "PatientUploads");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FacilityUploads");

            migrationBuilder.DropColumn(
                name: "UploadedAt",
                table: "FacilityUploads");

            migrationBuilder.AlterColumn<int>(
                name: "Visibility",
                table: "PatientUploads",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Public");

            migrationBuilder.AddColumn<Guid>(
                name: "PatientId1",
                table: "PatientUploads",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Visibility",
                table: "FacilityUploads",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Public");

            migrationBuilder.CreateIndex(
                name: "IX_PatientUploads_PatientId1",
                table: "PatientUploads",
                column: "PatientId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientUploads_Patients_PatientId1",
                table: "PatientUploads",
                column: "PatientId1",
                principalTable: "Patients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientUploads_Patients_PatientId1",
                table: "PatientUploads");

            migrationBuilder.DropIndex(
                name: "IX_PatientUploads_PatientId1",
                table: "PatientUploads");

            migrationBuilder.DropColumn(
                name: "PatientId1",
                table: "PatientUploads");

            migrationBuilder.AlterColumn<string>(
                name: "Visibility",
                table: "PatientUploads",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Public",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "PatientUploads",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedAt",
                table: "PatientUploads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Visibility",
                table: "FacilityUploads",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Public",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FacilityUploads",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadedAt",
                table: "FacilityUploads",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
