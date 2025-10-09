using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class makeremovecasadeforscheule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleDoctors_Doctors_DoctorId",
                table: "ScheduleDoctors");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleExceptionDoctors_Doctors_DoctorId",
                table: "ScheduleExceptionDoctors");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleDoctors_Doctors_DoctorId",
                table: "ScheduleDoctors",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleExceptionDoctors_Doctors_DoctorId",
                table: "ScheduleExceptionDoctors",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleDoctors_Doctors_DoctorId",
                table: "ScheduleDoctors");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduleExceptionDoctors_Doctors_DoctorId",
                table: "ScheduleExceptionDoctors");

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleDoctors_Doctors_DoctorId",
                table: "ScheduleDoctors",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduleExceptionDoctors_Doctors_DoctorId",
                table: "ScheduleExceptionDoctors",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
