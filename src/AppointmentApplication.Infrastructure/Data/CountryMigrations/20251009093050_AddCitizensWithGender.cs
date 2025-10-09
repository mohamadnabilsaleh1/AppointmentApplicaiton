using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppointmentApplication.Infrastructure.Data.CountryMigrations
{
    /// <inheritdoc />
    public partial class AddCitizensWithGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Citizens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationalId = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citizens", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Citizens",
                columns: new[] { "Id", "BirthDate", "FirstName", "Gender", "LastName", "MiddleName", "NationalId", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(1990, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ahmed", 0, "Ali", "Mohammed", 1000000001L, "0984306816" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(1988, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fatima", 1, "Yousef", "Hassan", 1000000002L, "0984306817" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(1995, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Omar", 0, "Mahmoud", "Khaled", 1000000003L, "0984306818" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(1992, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Layla", 1, "Abdel", "Sami", 1000000004L, "0984306819" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), new DateTime(1985, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Youssef", 0, "Hussein", "Adel", 1000000005L, "0984306820" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(1991, 6, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mariam", 1, "Saeed", "Tarek", 1000000006L, "0984306821" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(1989, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ali", 0, "Nabil", "Mostafa", 1000000007L, "0984306822" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(1994, 8, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sara", 1, "Fahmy", "Omar", 1000000008L, "0984306823" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(1993, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hassan", 0, "Kamal", "Ibrahim", 1000000009L, "0984306824" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(1990, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Noor", 1, "Salah", "Yahya", 1000000010L, "0984306825" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citizens");
        }
    }
}
