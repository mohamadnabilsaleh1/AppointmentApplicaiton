using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixRelationshiph : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthcareFacilities_FacilityId1",
                table: "HealthcareFacilityScheduleExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthcareFacilities_FacilityId1",
                table: "HealthcareFacilitySchedules");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityId1",
                table: "HealthcareFacilitySchedules",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityId1",
                table: "HealthcareFacilityScheduleExceptions",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthcareFacilities_FacilityId1",
                table: "HealthcareFacilityScheduleExceptions",
                column: "FacilityId1",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthcareFacilities_FacilityId1",
                table: "HealthcareFacilitySchedules",
                column: "FacilityId1",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthcareFacilities_FacilityId1",
                table: "HealthcareFacilityScheduleExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthcareFacilities_FacilityId1",
                table: "HealthcareFacilitySchedules");

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityId1",
                table: "HealthcareFacilitySchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FacilityId1",
                table: "HealthcareFacilityScheduleExceptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilityScheduleExceptions_HealthcareFacilities_FacilityId1",
                table: "HealthcareFacilityScheduleExceptions",
                column: "FacilityId1",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HealthcareFacilitySchedules_HealthcareFacilities_FacilityId1",
                table: "HealthcareFacilitySchedules",
                column: "FacilityId1",
                principalTable: "HealthcareFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
