using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class one : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HealthCareFacilityId",
                table: "FacilityUploads",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FacilityUploads_HealthCareFacilityId",
                table: "FacilityUploads",
                column: "HealthCareFacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FacilityUploads_HealthCareFacilities_HealthCareFacilityId",
                table: "FacilityUploads",
                column: "HealthCareFacilityId",
                principalTable: "HealthCareFacilities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FacilityUploads_HealthCareFacilities_HealthCareFacilityId",
                table: "FacilityUploads");

            migrationBuilder.DropIndex(
                name: "IX_FacilityUploads_HealthCareFacilityId",
                table: "FacilityUploads");

            migrationBuilder.DropColumn(
                name: "HealthCareFacilityId",
                table: "FacilityUploads");
        }
    }
}
