using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class AddVolumeLevelToMotherLullabyUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VolumeLevel",
                table: "MotherLullabyUsages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 1,
                column: "VolumeLevel",
                value: 50);

            migrationBuilder.UpdateData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 2,
                column: "VolumeLevel",
                value: 50);

            migrationBuilder.UpdateData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 3,
                column: "VolumeLevel",
                value: 50);

            migrationBuilder.UpdateData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 4,
                column: "VolumeLevel",
                value: 50);

            migrationBuilder.UpdateData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 5,
                column: "VolumeLevel",
                value: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VolumeLevel",
                table: "MotherLullabyUsages");
        }
    }
}
