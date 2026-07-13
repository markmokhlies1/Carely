using Carely.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class CryDetectionResultConfiguration : IEntityTypeConfiguration<CryDetectionResult>
    {
        public void Configure(EntityTypeBuilder<CryDetectionResult> builder)
        {
            builder.ToTable("CryDetectionResults");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsCrying)
                .IsRequired();

            builder.Property(x => x.DetectedAt)
                .IsRequired();

            builder.HasOne(x => x.DetectionSession)
                .WithMany()
                .HasForeignKey(x => x.DetectionSessionId)
                .OnDelete(DeleteBehavior.Cascade);


        }
        }
}
