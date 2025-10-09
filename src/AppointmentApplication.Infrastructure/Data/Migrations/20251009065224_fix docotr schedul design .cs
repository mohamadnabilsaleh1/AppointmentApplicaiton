using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixdocotrscheduldesign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleDoctors_Doctors_DoctorId1",
                table: "ScheduleDoctors");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleExceptionDoctors_Doctors_DoctorId1",
                table: "ScheduleExceptionDoctors");

            migrationBuilder.DropIndex(
                name: "IX_ScheduleDoctors_DoctorId1",
                table: "ScheduleDoctors");

            migrationBuilder.DropColumn(
                name: "DoctorId1",
                table: "ScheduleDoctors");

            migrationBuilder.AlterColumn<Guid>(
                name: "DoctorId1",
                table: "ScheduleExceptionDoctors",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "ScheduleExceptionDoctors",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleExceptionDoctors_Doctors_DoctorId1",
                table: "ScheduleExceptionDoctors",
                column: "DoctorId1",
                principalTable: "Doctors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleExceptionDoctors_Doctors_DoctorId1",
                table: "ScheduleExceptionDoctors");

            migrationBuilder.AlterColumn<Guid>(
                name: "DoctorId1",
                table: "ScheduleExceptionDoctors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "ScheduleExceptionDoctors",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<Guid>(
                name: "DoctorId1",
                table: "ScheduleDoctors",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleDoctors_DoctorId1",
                table: "ScheduleDoctors",
                column: "DoctorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleDoctors_Doctors_DoctorId1",
                table: "ScheduleDoctors",
                column: "DoctorId1",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleExceptionDoctors_Doctors_DoctorId1",
                table: "ScheduleExceptionDoctors",
                column: "DoctorId1",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
