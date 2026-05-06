using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MotherLullabyUsages",
                columns: new[] { "Id", "LastPosition", "LullabyId", "MotherId", "PlayCount" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 0, 0, 30, 0), 1, 2, 5 },
                    { 2, new TimeSpan(0, 0, 0, 45, 0), 2, 3, 3 },
                    { 3, new TimeSpan(0, 0, 1, 0, 0), 3, 4, 7 },
                    { 4, new TimeSpan(0, 0, 0, 20, 0), 1, 15, 4 },
                    { 5, new TimeSpan(0, 0, 0, 50, 0), 2, 16, 6 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MotherLullabyUsages",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
