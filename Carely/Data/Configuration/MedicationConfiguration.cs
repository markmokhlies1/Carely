using Carely.Models;
using Carely.Models.Enums.Medication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class MedicationConfiguration : IEntityTypeConfiguration<Medication>
    {
        public void Configure(EntityTypeBuilder<Medication> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(m => m.Description)
                   .HasMaxLength(500);

            builder.Property(m => m.Spot)
                   .IsRequired();

            builder.Property(m => m.StartDate)
                   .IsRequired();

            builder.Property(m => m.Duration)
                   .IsRequired();


            builder.Property(m => m.MedicationType)
                   .IsRequired();


            builder.HasOne(m => m.Mother)
                   .WithMany(m => m.Medications)
                   .HasForeignKey(m => m.MotherId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(LoadData());
        }

        private static List<Medication> LoadData()
        {
            return new List<Medication>()
            {
                new Medication
                {
                    Id = 1,
                    Name = "Vitamin D",
                    Description = "Daily vitamin supplement for the baby.",
                    Spot = Spot.Morning,
                    StartDate = new DateTime(2025, 1, 1),
                    Duration = 30,
                    MedicationType = MedicationType.Drink,
                    MotherId = 1
                },
                new Medication
                {
                    Id = 2,
                    Name = "Cough Syrup",
                    Description = "Taken after meals to relieve cough.",
                    Spot = Spot.Morning,
                    StartDate = new DateTime(2025, 2, 10),
                    Duration = 10,
                    MedicationType = MedicationType.Drink,
                    MotherId = 2
                },
                new Medication
                {
                    Id = 3,
                    Name = "Antibiotic Injection",
                    Description = "Prescribed for infection treatment.",
                    Spot = Spot.Morning,
                    StartDate = new DateTime(2025, 3, 5),
                    Duration = 7,
                    MedicationType = MedicationType.Injection,
                    MotherId = 3
                }
            };
        }
    }
}
