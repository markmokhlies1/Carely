using Carely.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class MotherLullabyUsageConfiguration : IEntityTypeConfiguration<MotherLullabyUsage>
    {
        public void Configure(EntityTypeBuilder<MotherLullabyUsage> builder)
        {
            builder.ToTable("MotherLullabyUsages");

            builder.HasKey(mu => mu.Id);

            builder.Property(mu => mu.PlayCount)
                   .IsRequired();

            builder.Property(mu => mu.LastPosition)
                   .HasColumnType("time");

            // Relationship: Mother ↔ MotherLullabyUsage
            builder.HasOne(mu => mu.Mother)
                   .WithMany(m => m.LullabyUsages)
                   .HasForeignKey(mu => mu.MotherId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Lullaby ↔ MotherLullabyUsage
            builder.HasOne(mu => mu.Lullaby)
                   .WithMany(l => l.MotherUsages)
                   .HasForeignKey(mu => mu.LullabyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(LoadData());
        }


        private MotherLullabyUsage[] LoadData()
        {
            return new MotherLullabyUsage[]
            {
                //new MotherLullabyUsage
                //{
                //    Id = 1,
                //    MotherId = 1,
                //    LullabyId = 1,
                //    PlayCount = 5,
                //    LastPosition = TimeSpan.FromSeconds(30)
                //},
                //new MotherLullabyUsage
                //{
                //    Id = 2,
                //    MotherId = 2,
                //    LullabyId = 2,
                //    PlayCount = 3,
                //    LastPosition = TimeSpan.FromSeconds(45)
                //},
                //new MotherLullabyUsage
                //{
                //    Id = 3,
                //    MotherId = 3,
                //    LullabyId = 3,
                //    PlayCount = 7,
                //    LastPosition = TimeSpan.FromMinutes(1)
                //},
                //new MotherLullabyUsage
                //{
                //    Id = 4,
                //    MotherId = 1,
                //    LullabyId = 2,
                //    PlayCount = 2,
                //    LastPosition = null
                //}
                
               
                new MotherLullabyUsage
                {
                    Id = 1,
                    MotherId = 2, // Nada
                    LullabyId = 1,
                    PlayCount = 5,
                    LastPosition = TimeSpan.FromSeconds(30)
                },
                new MotherLullabyUsage
                {
                    Id = 2,
                    MotherId = 3, // Eman
                    LullabyId = 2,
                    PlayCount = 3,
                    LastPosition = TimeSpan.FromSeconds(45)
                },
                new MotherLullabyUsage
                {
                    Id = 3,
                    MotherId = 4, // Aya
                    LullabyId = 3,
                    PlayCount = 7,
                    LastPosition = TimeSpan.FromMinutes(1)
                },
                new MotherLullabyUsage
                {
                    Id = 4,
                    MotherId = 15, // Salma
                    LullabyId = 1,
                    PlayCount = 4,
                    LastPosition = TimeSpan.FromSeconds(20)
                },
                new MotherLullabyUsage
                {
                    Id = 5,
                    MotherId = 16, // Sama
                    LullabyId = 2,
                    PlayCount = 6,
                    LastPosition = TimeSpan.FromSeconds(50)
                }
            };
        }
    }
}