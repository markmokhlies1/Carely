using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class RecreateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Mothers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.CreateTable(
                name: "Lullabies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    LastPosition = table.Column<TimeSpan>(type: "time", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lullabies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MotherLullabyUsages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MotherId = table.Column<int>(type: "int", nullable: false),
                    LullabyId = table.Column<int>(type: "int", nullable: false),
                    PlayCount = table.Column<int>(type: "int", nullable: false),
                    LastPosition = table.Column<TimeSpan>(type: "time", nullable: true)
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

            //migrationBuilder.InsertData(
            //    table: "Mothers",
            //    columns: new[] { "Id", "BirthDate", "Email", "FirstName", "Hight", "LastName", "PasswordHash", "PhoneNumber", "Role", "Weight" },
            //    values: new object[,]
            //    {
            //        { 4, new DateTime(2000, 10, 20, 17, 8, 33, 0, DateTimeKind.Unspecified), "aya@gmail.com", "Aya", 170, "Mohamed", "Aya@123", "01155456811", 0, 80 },
            //        { 15, new DateTime(2004, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "salmasheh69@gmail.com", "Salma", 160, "Shehab", "Salma@123", "01010529873", 0, 63 },
            //        { 16, new DateTime(2004, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "sama2323@gmail.com", "Sama", 160, "Ahmed", "Sama@123", "01234678549", 0, 55 }
            //    });

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

            migrationBuilder.DropTable(
                name: "Lullabies");

            //migrationBuilder.DeleteData(
            //    table: "Mothers",
            //    keyColumn: "Id",
            //    keyValue: 4);

            //migrationBuilder.DeleteData(
            //    table: "Mothers",
            //    keyColumn: "Id",
            //    keyValue: 15);

            //migrationBuilder.DeleteData(
            //    table: "Mothers",
            //    keyColumn: "Id",
            //    keyValue: 16);

            //migrationBuilder.InsertData(
            //    table: "Mothers",
            //    columns: new[] { "Id", "BirthDate", "Email", "FirstName", "Hight", "LastName", "PasswordHash", "PhoneNumber", "Role", "Weight" },
            //    values: new object[] { 1, new DateTime(1998, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "sara@example.com", "Sara", 165, "Khaled", "123456", "01112345678", 0, 62 });
        }
    }
}
