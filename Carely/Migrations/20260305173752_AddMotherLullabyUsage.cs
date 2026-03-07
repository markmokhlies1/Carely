using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class AddMotherLullabyUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lullabies_Mothers_MotherId",
                table: "Lullabies");

            migrationBuilder.DropIndex(
                name: "IX_Lullabies_MotherId",
                table: "Lullabies");

            migrationBuilder.DropColumn(
                name: "MotherId",
                table: "Lullabies");

            migrationBuilder.CreateTable(
                name: "MotherLullabyUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MotherId = table.Column<int>(type: "int", nullable: false),
                    LullabyId = table.Column<int>(type: "int", nullable: false),
                    PlayCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotherLullabyUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MotherLullabyUsages_Lullabies_LullabyId",
                        column: x => x.LullabyId,
                        principalTable: "Lullabies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MotherLullabyUsages_Mothers_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Mothers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotherLullabyUsages_LullabyId",
                table: "MotherLullabyUsages",
                column: "LullabyId");

            migrationBuilder.CreateIndex(
                name: "IX_MotherLullabyUsages_MotherId",
                table: "MotherLullabyUsages",
                column: "MotherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotherLullabyUsages");

            migrationBuilder.AddColumn<int>(
                name: "MotherId",
                table: "Lullabies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Lullabies",
                keyColumn: "Id",
                keyValue: 1,
                column: "MotherId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Lullabies",
                keyColumn: "Id",
                keyValue: 2,
                column: "MotherId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Lullabies",
                keyColumn: "Id",
                keyValue: 3,
                column: "MotherId",
                value: 3);

            migrationBuilder.CreateIndex(
                name: "IX_Lullabies_MotherId",
                table: "Lullabies",
                column: "MotherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lullabies_Mothers_MotherId",
                table: "Lullabies",
                column: "MotherId",
                principalTable: "Mothers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
