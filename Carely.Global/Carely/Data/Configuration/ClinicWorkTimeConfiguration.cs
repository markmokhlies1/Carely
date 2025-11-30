using Carely.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class ClinicWorkTimeConfiguration : IEntityTypeConfiguration<ClinicWorkTime>
    {
        public void Configure(EntityTypeBuilder<ClinicWorkTime> builder)
        {
            builder.ToTable("ClinicWorkTimes");

            builder.HasKey(w => w.Id);

            builder.Property(w => w.Day)
                   .IsRequired();

            builder.Property(w => w.From)
                   .IsRequired();

            builder.Property(w => w.To)
                   .IsRequired();

            builder.Property(w => w.ClinicId)
                   .IsRequired();

            builder.HasOne(w => w.Clinic)
                   .WithMany(c => c.WorkTimes)
                   .HasForeignKey(w => w.ClinicId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 
