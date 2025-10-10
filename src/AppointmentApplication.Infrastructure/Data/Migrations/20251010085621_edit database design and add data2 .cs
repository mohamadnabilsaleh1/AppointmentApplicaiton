using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class editdatabasedesignandadddata2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "ChronicDiseases");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ChronicDiseases");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ChronicDiseases");

            migrationBuilder.DropColumn(
                name: "UpdatedAtdUtc",
                table: "ChronicDiseases");

            migrationBuilder.InsertData(
                table: "ChronicDiseases",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), 0 },
                    { new Guid("00000000-0000-0000-0000-000000000002"), 1 },
                    { new Guid("00000000-0000-0000-0000-000000000003"), 2 },
                    { new Guid("00000000-0000-0000-0000-000000000004"), 3 },
                    { new Guid("00000000-0000-0000-0000-000000000005"), 4 },
                    { new Guid("00000000-0000-0000-0000-000000000006"), 5 },
                    { new Guid("00000000-0000-0000-0000-000000000007"), 6 },
                    { new Guid("00000000-0000-0000-0000-000000000008"), 7 },
                    { new Guid("00000000-0000-0000-0000-000000000009"), 8 },
                    { new Guid("00000000-0000-0000-0000-000000000010"), 9 },
                    { new Guid("00000000-0000-0000-0000-000000000011"), 10 },
                    { new Guid("00000000-0000-0000-0000-000000000012"), 11 },
                    { new Guid("00000000-0000-0000-0000-000000000013"), 12 },
                    { new Guid("00000000-0000-0000-0000-000000000014"), 13 },
                    { new Guid("00000000-0000-0000-0000-000000000015"), 14 },
                    { new Guid("00000000-0000-0000-0000-000000000016"), 15 },
                    { new Guid("00000000-0000-0000-0000-000000000017"), 16 },
                    { new Guid("00000000-0000-0000-0000-000000000018"), 17 },
                    { new Guid("00000000-0000-0000-0000-000000000019"), 18 },
                    { new Guid("00000000-0000-0000-0000-000000000020"), 19 },
                    { new Guid("00000000-0000-0000-0000-000000000021"), 20 },
                    { new Guid("00000000-0000-0000-0000-000000000022"), 21 },
                    { new Guid("00000000-0000-0000-0000-000000000023"), 22 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000021"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "ChronicDiseases",
                keyColumn: "Id",
                keyValue: new Guid("00000000-0000-0000-0000-000000000023"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "ChronicDiseases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ChronicDiseases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "ChronicDiseases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtdUtc",
                table: "ChronicDiseases",
                type: "datetime2",
                nullable: true);
        }
    }
}
