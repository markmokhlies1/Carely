using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class InitAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lullabies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    LastPosition = table.Column<TimeSpan>(type: "time", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lullabies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lullabies_Mothers_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Mothers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Lullabies",
                columns: new[] { "Id", "Duration", "FilePath", "LastPosition", "MotherId", "Name" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 0, 2, 7, 0), "audio/lullabies/bells.mp3", null, 1, "bells" },
                    { 2, new TimeSpan(0, 0, 1, 16, 0), "audio/lullabies/sleeping.mp3", null, 2, "sleeping" },
                    { 3, new TimeSpan(0, 0, 3, 58, 0), "audio/lullabies/whale.mp3", null, 3, "whale" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Lullabies_MotherId",
                table: "Lullabies",
                column: "MotherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Lullabies");
        }
    }
}
