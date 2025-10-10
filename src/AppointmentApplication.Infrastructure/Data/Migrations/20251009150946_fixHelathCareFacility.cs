using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixHelathCareFacility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HealthCareFacilities_users_UserId1",
                table: "HealthCareFacilities");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_users_UserId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_HealthCareFacilities_UserId1",
                table: "HealthCareFacilities");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "HealthCareFacilities");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_users_UserId",
                table: "Patients",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_users_UserId",
                table: "Patients");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "HealthCareFacilities",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HealthCareFacilities_UserId1",
                table: "HealthCareFacilities",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HealthCareFacilities_users_UserId1",
                table: "HealthCareFacilities",
                column: "UserId1",
                principalTable: "users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_users_UserId",
                table: "Patients",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id");
        }
    }
}
