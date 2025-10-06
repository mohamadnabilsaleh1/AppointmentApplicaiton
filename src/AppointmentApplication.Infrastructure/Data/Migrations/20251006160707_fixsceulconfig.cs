using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixsceulconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthCareFacilities_FacilityId",
                table: "HealthcareFacilitySchedules");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthCareFacilities_FacilityId",
                table: "HealthcareFacilitySchedules",
                column: "FacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthCareFacilities_FacilityId",
                table: "HealthcareFacilitySchedules");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthCareFacilities_FacilityId",
                table: "HealthcareFacilitySchedules",
                column: "FacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
