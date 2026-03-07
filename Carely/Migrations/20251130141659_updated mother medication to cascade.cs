using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class updatedmothermedicationtocascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Mothers_MotherId",
                table: "Medications");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Mothers_MotherId",
                table: "Medications",
                column: "MotherId",
                principalTable: "Mothers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_Mothers_MotherId",
                table: "Medications");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_Mothers_MotherId",
                table: "Medications",
                column: "MotherId",
                principalTable: "Mothers",
                principalColumn: "Id");
        }
    }
}
