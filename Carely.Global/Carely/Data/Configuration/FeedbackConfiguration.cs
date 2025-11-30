using Carely.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable("Feedbacks");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Stars)
                   .IsRequired();

            builder.Property(f => f.Comment)
                   .HasMaxLength(500);

            builder.Property(f => f.MotherId)
                   .IsRequired();

            builder.Property(f => f.MeetingId)
                   .IsRequired();

            builder.HasOne(f => f.Mother)
                   .WithMany(m => m.Feedbacks)
                   .HasForeignKey(f => f.MotherId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Meeting)
                   .WithMany(m => m.Feedbacks)
                   .HasForeignKey(f => f.MeetingId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 
