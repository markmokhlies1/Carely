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
                Name = "forest lullaby",
                Duration = TimeSpan.FromMinutes(2).Add(TimeSpan.FromSeconds(18)),
                FilePath = "audio/lullabies/forest.mp3",

            },
            new Lullaby
            {
                Id = 2,
                Name = "sleeping lullaby",
                Duration = TimeSpan.FromMinutes(1).Add(TimeSpan.FromSeconds(16)),
                FilePath = "audio/lullabies/sleeping.mp3",

            },
            new Lullaby
            {
                Id = 3,
                Name = "silentvoice lullaby",
                Duration = TimeSpan.FromMinutes(4).Add(TimeSpan.FromSeconds(20)),
                FilePath = "audio/lullabies/silentvoice.mp3",

            }
        };
        }
    }
}
