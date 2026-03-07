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
        }
    }
}
