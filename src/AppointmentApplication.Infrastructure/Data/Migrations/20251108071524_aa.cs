using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class aa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emails_users_UserId",
                table: "Emails");

            migrationBuilder.DropForeignKey(
                name: "FK_Phones_users_UserId",
                table: "Phones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Emails",
                table: "Emails");

            migrationBuilder.RenameTable(
                name: "Emails",
                newName: "emails");

            migrationBuilder.RenameIndex(
                name: "IX_Emails_UserId_IsPrimary",
                table: "emails",
                newName: "IX_emails_UserId_IsPrimary");

            migrationBuilder.RenameIndex(
                name: "IX_Emails_UserId",
                table: "emails",
                newName: "IX_emails_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_emails",
                table: "emails",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_emails_EmailAddress",
                table: "emails",
                column: "EmailAddress");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_emails_users_UserId",
                table: "emails");

            migrationBuilder.DropForeignKey(
                name: "FK_Phones_users_UserId",
                table: "Phones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_emails",
                table: "emails");

            migrationBuilder.DropIndex(
                name: "IX_emails_EmailAddress",
                table: "emails");

            migrationBuilder.RenameTable(
                name: "emails",
                newName: "Emails");

            migrationBuilder.RenameIndex(
                name: "IX_emails_UserId_IsPrimary",
                table: "Emails",
                newName: "IX_Emails_UserId_IsPrimary");

            migrationBuilder.RenameIndex(
                name: "IX_emails_UserId",
                table: "Emails",
                newName: "IX_Emails_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Emails",
                table: "Emails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Emails_users_UserId",
                table: "Emails",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Phones_users_UserId",
                table: "Phones",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
