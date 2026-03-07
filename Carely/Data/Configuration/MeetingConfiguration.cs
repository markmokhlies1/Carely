using Carely.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class MeetingConfiguration : IEntityTypeConfiguration<Meeting>
    {
        public void Configure(EntityTypeBuilder<Meeting> builder)
        {
            builder.ToTable("Meetings");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Description)
                   .HasMaxLength(500);

            builder.Property(m => m.MeetingType)
                   .IsRequired();

            builder.Property(m => m.Date)
                   .IsRequired();

            builder.Property(m => m.Address)
                   .HasMaxLength(200);

            builder.Property(m => m.DoctorId)
                   .IsRequired();

            builder.HasOne(m => m.Doctor)
                   .WithMany(d => d.Meetings)
                   .HasForeignKey(m => m.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.Feedbacks)
                   .WithOne(f => f.Meeting)
                   .HasForeignKey(f => f.MeetingId)
                   .OnDelete(DeleteBehavior.Cascade);

            
            builder.HasMany(m => m.Mothers)
                   .WithMany(mo => mo.Meetings)
                   .UsingEntity(j =>
                        j.ToTable("MeetingMothers"));  
        }
    }
} 
