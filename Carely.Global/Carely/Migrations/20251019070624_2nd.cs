using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class _2nd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Mothers",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Admins",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Mothers",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Admins",
                newName: "Password");
        }
    }
}
