using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class done : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_emails_users_UserId",
                table: "emails");

            migrationBuilder.DropForeignKey(
                name: "FK_Phones_users_UserId",
                table: "Phones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Phones",
                table: "Phones");

            migrationBuilder.RenameTable(
                name: "Phones",
                newName: "phones");

            migrationBuilder.RenameIndex(
                name: "IX_Phones_UserId_IsPrimary",
                table: "phones",
                newName: "IX_phones_UserId_IsPrimary");

            migrationBuilder.AddPrimaryKey(
                name: "PK_phones",
                table: "phones",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_phones_PhoneNumber",
                table: "phones",
                column: "PhoneNumber");

            migrationBuilder.CreateIndex(
                name: "IX_phones_UserId",
                table: "phones",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_emails_users_UserId",
                table: "emails",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_phones_users_UserId",
                table: "phones",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_emails_users_UserId",
                table: "emails");

            migrationBuilder.DropForeignKey(
                name: "FK_phones_users_UserId",
                table: "phones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_phones",
                table: "phones");

            migrationBuilder.DropIndex(
                name: "IX_phones_PhoneNumber",
                table: "phones");

            migrationBuilder.DropIndex(
                name: "IX_phones_UserId",
                table: "phones");

            migrationBuilder.RenameTable(
                name: "phones",
                newName: "Phones");

            migrationBuilder.RenameIndex(
                name: "IX_phones_UserId_IsPrimary",
                table: "Phones",
                newName: "IX_Phones_UserId_IsPrimary");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Phones",
                table: "Phones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_emails_users_UserId",
                table: "emails",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Phones_users_UserId",
                table: "Phones",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
