using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class AddCryDetectionResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CryDetectionResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsCrying = table.Column<bool>(type: "bit", nullable: false),
                    DetectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DetectionSessionId = table.Column<int>(type: "int", nullable: false),
                    DetectionSessionId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryDetectionResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CryDetectionResults_DetectionSessions_DetectionSessionId",
                        column: x => x.DetectionSessionId,
                        principalTable: "DetectionSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CryDetectionResults_DetectionSessions_DetectionSessionId1",
                        column: x => x.DetectionSessionId1,
                        principalTable: "DetectionSessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CryDetectionResults_DetectionSessionId",
                table: "CryDetectionResults",
                column: "DetectionSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_CryDetectionResults_DetectionSessionId1",
                table: "CryDetectionResults",
                column: "DetectionSessionId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryDetectionResults");
        }
    }
}
