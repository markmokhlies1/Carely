using Carely.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Carely.Data.Configuration
{
    public class LullabyConfiguration : IEntityTypeConfiguration<Lullaby>
    {
        public void Configure(EntityTypeBuilder<Lullaby> builder)
        {
            builder.ToTable("Lullabies");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(l => l.Duration)
                   .IsRequired()
                   .HasColumnType("time");

            builder.Property(l => l.LastPosition)
                   .HasColumnType("time");

            builder.Property(l => l.FilePath)
                   .IsRequired();

            //builder.HasOne(l => l.Mother)
            //       .WithMany(m => m.Lullabies)
            //       .HasForeignKey(l => l.MotherId)
            //       .OnDelete(DeleteBehavior.Cascade);\

            builder.HasMany(l => l.MotherUsages)
                .WithOne(mu => mu.Lullaby)
                .HasForeignKey(mu => mu.LullabyId)
                .OnDelete(DeleteBehavior.Cascade);
 
            
            builder.HasData(LoadData());
        }

        private static List<Lullaby> LoadData()
        {
            return new List<Lullaby>
        {
            new Lullaby
            {
                Id = 1,
                Name = "bells",
                Duration = TimeSpan.FromMinutes(2).Add(TimeSpan.FromSeconds(7)),
                FilePath = "audio/lullabies/bells.mp3",
                //MotherId = 1
            },
            new Lullaby
            {
                Id = 2,
                Name = "sleeping",
                Duration = TimeSpan.FromMinutes(1).Add(TimeSpan.FromSeconds(16)),
                FilePath = "audio/lullabies/sleeping.mp3",
                //MotherId = 2
            },
            new Lullaby
            {
                Id = 3,
                Name = "whale",
                Duration = TimeSpan.FromMinutes(3).Add(TimeSpan.FromSeconds(58)),
                FilePath = "audio/lullabies/whale.mp3",
                //MotherId = 3
            }
        };
        }
    }
}
