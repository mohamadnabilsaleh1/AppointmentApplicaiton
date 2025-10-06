using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DeleteIsActiveFromDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthCareFacilities_FacilityId",
                table: "HealthcareFacilityScheduleExceptions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Departments");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthCareFacilities_FacilityId",
                table: "HealthcareFacilityScheduleExceptions",
                column: "FacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthCareFacilities_FacilityId",
                table: "HealthcareFacilityScheduleExceptions");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthCareFacilities_FacilityId",
                table: "HealthcareFacilityScheduleExceptions",
                column: "FacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
