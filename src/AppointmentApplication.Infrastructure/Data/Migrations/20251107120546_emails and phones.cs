using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentApplication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class emailsandphones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Phones_OwnerType_OwnerId",
                table: "Phones");

            migrationBuilder.DropIndex(
                name: "IX_Emails_OwnerType_OwnerId",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "OwnerType",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "OwnerType",
                table: "Emails");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Phones",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Emails",
                newName: "UserId");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "Phones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "Emails",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Phones_UserId_IsPrimary",
                table: "Phones",
                columns: new[] { "UserId", "IsPrimary" },
                unique: true,
                filter: "[IsPrimary] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Emails_UserId",
                table: "Emails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Emails_UserId_IsPrimary",
                table: "Emails",
                columns: new[] { "UserId", "IsPrimary" },
                unique: true,
                filter: "[IsPrimary] = 1");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emails_users_UserId",
                table: "Emails");

            migrationBuilder.DropForeignKey(
                name: "FK_Phones_users_UserId",
                table: "Phones");

            migrationBuilder.DropIndex(
                name: "IX_Phones_UserId_IsPrimary",
                table: "Phones");

            migrationBuilder.DropIndex(
                name: "IX_Emails_UserId",
                table: "Emails");

            migrationBuilder.DropIndex(
                name: "IX_Emails_UserId_IsPrimary",
                table: "Emails");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "Emails");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Phones",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Emails",
                newName: "OwnerId");

            migrationBuilder.AddColumn<int>(
                name: "OwnerType",
                table: "Phones",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerType",
                table: "Emails",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Phones_OwnerType_OwnerId",
                table: "Phones",
                columns: new[] { "OwnerType", "OwnerId" });

            migrationBuilder.CreateIndex(
                name: "IX_Emails_OwnerType_OwnerId",
                table: "Emails",
                columns: new[] { "OwnerType", "OwnerId" });
        }
    }
}
