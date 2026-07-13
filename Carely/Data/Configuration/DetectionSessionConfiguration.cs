using Carely.Models;
using Carely.Models.Enums.DetectionSession;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class DetectionSessionConfiguration : IEntityTypeConfiguration<DetectionSession>
    {
        public void Configure(EntityTypeBuilder<DetectionSession> builder)
        {
            builder.ToTable("DetectionSessions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.StartTime).IsRequired();

            builder.Property(x => x.EndTime).IsRequired(false);

            builder.Property(b => b.Status)
            .IsRequired();

            builder.HasOne(x => x.Baby)
                .WithMany(b => b.DetectionSessions)
                .HasForeignKey(x => x.BabyId)
                .OnDelete(DeleteBehavior.Cascade);


        }
        }
}
