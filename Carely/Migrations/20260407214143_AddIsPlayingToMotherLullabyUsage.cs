using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPlayingToMotherLullabyUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPlaying",
                table: "MotherLullabyUsages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsPlaying",
                value: false);

            migrationBuilder.UpdateData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsPlaying",
                value: false);

            migrationBuilder.UpdateData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 3,
                column: "IsPlaying",
                value: false);

            migrationBuilder.UpdateData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 4,
                column: "IsPlaying",
                value: false);

            migrationBuilder.UpdateData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 5,
                column: "IsPlaying",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPlaying",
                table: "MotherLullabyUsages");
        }
    }
}
