using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.DeleteData(
            //        table: "Mothers",
            //        keyColumn: "Id",
            //        keyValue: 2);

            //    migrationBuilder.DeleteData(
            //        table: "Mothers",
            //        keyColumn: "Id",
            //        keyValue: 3);

            //    migrationBuilder.DeleteData(
            //        table: "Mothers",
            //        keyColumn: "Id",
            //        keyValue: 4);

            //    migrationBuilder.DeleteData(
            //        table: "Mothers",
            //        keyColumn: "Id",
            //        keyValue: 15);

            //    migrationBuilder.DeleteData(
            //        table: "Mothers",
            //        keyColumn: "Id",
            //        keyValue: 16);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.InsertData(
            //        table: "Mothers",
            //        columns: new[] { "Id", "BirthDate", "Email", "FirstName", "Hight", "LastName", "PasswordHash", "PhoneNumber", "Role", "Weight" },
            //        values: new object[,]
            //        {
            //            { 2, new DateTime(1995, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "nada@example.com", "Nada", 160, "Mohsen", "654321", "01098765432", 0, 58 },
            //            { 3, new DateTime(2000, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "eman@example.com", "Eman", 170, "Ali", "987654", "01234567890", 0, 70 },
            //            { 4, new DateTime(2000, 10, 20, 17, 8, 33, 0, DateTimeKind.Unspecified), "aya@gmail.com", "Aya", 170, "Mohamed", "Aya@123", "01155456811", 0, 80 },
            //            { 15, new DateTime(2004, 9, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "salmasheh69@gmail.com", "Salma", 160, "Shehab", "Salma@123", "01010529873", 0, 63 },
            //            { 16, new DateTime(2004, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "sama2323@gmail.com", "Sama", 160, "Ahmed", "Sama@123", "01234678549", 0, 55 }
            //        });
        }
    }
}
