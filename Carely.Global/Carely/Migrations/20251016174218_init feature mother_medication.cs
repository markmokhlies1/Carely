using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class initfeaturemother_medication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mothers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hight = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mothers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Spot = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    MedicationType = table.Column<int>(type: "int", nullable: false),
                    MotherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medications_Mothers_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Mothers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "Role" },
                values: new object[,]
                {
                    { 1, "super.admin@babycare.com", "Super", "Admin", "Admin@123", "01000000000", 1 },
                    { 2, "mona.admin@babycare.com", "Mona", "Adel", "Mona@123", "01011111111", 1 },
                    { 3, "hassan.admin@babycare.com", "Hassan", "Tarek", "Hassan@123", "01022222222", 1 }
                });

            migrationBuilder.InsertData(
                table: "Mothers",
                columns: new[] { "Id", "BirthDate", "Email", "FirstName", "Hight", "LastName", "Password", "PhoneNumber", "Role", "Weight" },
                values: new object[,]
                {
                    { 1, new DateTime(1998, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "sara@example.com", "Sara", 165, "Khaled", "123456", "01112345678", 0, 62 },
                    { 2, new DateTime(1995, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "nada@example.com", "Nada", 160, "Mohsen", "654321", "01098765432", 0, 58 },
                    { 3, new DateTime(2000, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "eman@example.com", "Eman", 170, "Ali", "987654", "01234567890", 0, 70 }
                });

            migrationBuilder.InsertData(
                table: "Medications",
                columns: new[] { "Id", "Description", "Duration", "MedicationType", "MotherId", "Name", "Spot", "StartDate" },
                values: new object[,]
                {
                    { 1, "Daily vitamin supplement for the baby.", 30, 2, 1, "Vitamin D", 0, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Taken after meals to relieve cough.", 10, 2, 2, "Cough Syrup", 0, new DateTime(2025, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Prescribed for infection treatment.", 7, 0, 3, "Antibiotic Injection", 0, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Medications_MotherId",
                table: "Medications",
                column: "MotherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropTable(
                name: "Mothers");
        }
    }
}
