using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class editdatabasedesign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAllergies_Allergies_AllergyId",
                table: "PatientAllergies");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAllergies_Patients_PatientId",
                table: "PatientAllergies");

            migrationBuilder.DropTable(
                name: "PatientChronicDisease");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientAllergies",
                table: "PatientAllergies");

            migrationBuilder.DropIndex(
                name: "IX_PatientAllergies_PatientId_AllergyId",
                table: "PatientAllergies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChronicDisease",
                table: "ChronicDisease");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PatientAllergies");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "PatientAllergies");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "PatientAllergies");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "PatientAllergies");

            migrationBuilder.DropColumn(
                name: "UpdatedAtdUtc",
                table: "PatientAllergies");

            migrationBuilder.RenameTable(
                name: "ChronicDisease",
                newName: "ChronicDiseases");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientAllergies",
                table: "PatientAllergies",
                columns: new[] { "PatientId", "AllergyId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChronicDiseases",
                table: "ChronicDiseases",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PatientChronicDiseases",
                columns: table => new
                {
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChronicDiseaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientChronicDiseases", x => new { x.PatientId, x.ChronicDiseaseId });
                    table.ForeignKey(
                        name: "FK_PatientChronicDiseases_ChronicDiseases_ChronicDiseaseId",
                        column: x => x.ChronicDiseaseId,
                        principalTable: "ChronicDiseases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientChronicDiseases_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientChronicDiseases_ChronicDiseaseId",
                table: "PatientChronicDiseases",
                column: "ChronicDiseaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAllergies_Allergies_AllergyId",
                table: "PatientAllergies",
                column: "AllergyId",
                principalTable: "Allergies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAllergies_Patients_PatientId",
                table: "PatientAllergies",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientAllergies_Allergies_AllergyId",
                table: "PatientAllergies");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientAllergies_Patients_PatientId",
                table: "PatientAllergies");

            migrationBuilder.DropTable(
                name: "PatientChronicDiseases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PatientAllergies",
                table: "PatientAllergies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChronicDiseases",
                table: "ChronicDiseases");

            migrationBuilder.RenameTable(
                name: "ChronicDiseases",
                newName: "ChronicDisease");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PatientAllergies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "PatientAllergies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "PatientAllergies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "PatientAllergies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "PatientAllergies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PatientAllergies",
                table: "PatientAllergies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChronicDisease",
                table: "ChronicDisease",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PatientChronicDisease",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChronicDiseaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAtdUtc = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientChronicDisease", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientChronicDisease_ChronicDisease_ChronicDiseaseId",
                        column: x => x.ChronicDiseaseId,
                        principalTable: "ChronicDisease",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientChronicDisease_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientAllergies_PatientId_AllergyId",
                table: "PatientAllergies",
                columns: new[] { "PatientId", "AllergyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientChronicDisease_ChronicDiseaseId",
                table: "PatientChronicDisease",
                column: "ChronicDiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientChronicDisease_PatientId_ChronicDiseaseId",
                table: "PatientChronicDisease",
                columns: new[] { "PatientId", "ChronicDiseaseId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAllergies_Allergies_AllergyId",
                table: "PatientAllergies",
                column: "AllergyId",
                principalTable: "Allergies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientAllergies_Patients_PatientId",
                table: "PatientAllergies",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
