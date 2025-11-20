using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class asdfesdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Doctors_DoctorID",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CreatedAtUtc",
                table: "Reviews",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_DoctorID_CreatedAtUtc",
                table: "Reviews",
                columns: new[] { "DoctorID", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_Rating",
                table: "Reviews",
                column: "Rating");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_Rating",
                table: "Reviews",
                sql: "[Rating] BETWEEN 1 AND 5");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Doctors_DoctorID",
                table: "Reviews",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Doctors_DoctorID",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_CreatedAtUtc",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_DoctorID_CreatedAtUtc",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_Rating",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_Rating",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Doctors_DoctorID",
                table: "Reviews",
                column: "DoctorID",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
