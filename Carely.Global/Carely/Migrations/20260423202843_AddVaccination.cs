using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Carely.Migrations
{
    /// <inheritdoc />
    public partial class AddVaccination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vaccinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Dosage = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Disease = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccinations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Vaccinations",
                columns: new[] { "Id", "Age", "Disease", "Dosage", "Name" },
                values: new object[,]
                {
                    { 1, 0, "Hepatitis B", 0, "Liver B infant" },
                    { 2, 0, "Polio", 1, "Sabine" },
                    { 3, 0, "Tuberculosis", 2, "BCG" },
                    { 4, 1, "Polio", 3, "Sabine" },
                    { 5, 1, "Diphtheria, pertussis, tetanus, hepatitis B and influenzae influenzae Hemorrhagic TypeB", 3, "The taste of the pentagram" },
                    { 6, 1, "Paralyzed polio", 3, "Salk's Taste" },
                    { 7, 2, "Polio", 4, "Sabine" },
                    { 8, 2, "Diphtheria, whooping cough, tetanus, hepatitis B and haemorrhagic influenzae StyleB", 4, "The taste of the pentagram" },
                    { 9, 2, "Paralyzed polio", 4, "Taste of Soulk" },
                    { 10, 3, "Polio", 5, "Sabine" },
                    { 11, 3, "Diphtheria, pertussis, tetanus, hepatitis B and influenzae influenzae Hemorrhagic TypeB", 5, "The taste of the pentagram" },
                    { 12, 3, "Paralyzed polio", 5, "Taste of Soulk" },
                    { 13, 4, "Polio", 6, "Sabine" },
                    { 14, 5, "Polio", 7, "Sabine" },
                    { 15, 5, "Measles, mumps and rubella", 8, "Viral MMR" },
                    { 16, 6, "Polio", 8, "Sabine" },
                    { 17, 6, "Measles, mumps and rubella", 8, "Viral MMR" },
                    { 18, 6, "Diphtheria, tetanus, and whooping cough", 8, "Bacterial triad" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vaccinations");
        }
    }
}
