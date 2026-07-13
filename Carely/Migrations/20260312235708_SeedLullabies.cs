using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class SeedLullabies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lullabies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Lullabies",
                columns: new[] { "Id", "Duration", "FilePath", "LastPosition", "Name" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 0, 2, 18, 0), "audio/lullabies/forest.mp3", null, "forest lullaby" },
                    { 2, new TimeSpan(0, 0, 1, 16, 0), "audio/lullabies/sleeping.mp3", null, "sleeping lullaby" },
                    { 3, new TimeSpan(0, 0, 4, 20, 0), "audio/lullabies/silentvoice.mp3", null, "silentvoice lullaby" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Lullabies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Lullabies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Lullabies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Lullabies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
