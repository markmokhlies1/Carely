using Carely.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class ClinicConfiguration : IEntityTypeConfiguration<Clinic>
    {
        public void Configure(EntityTypeBuilder<Clinic> builder)
        {
            builder.ToTable("Clinics");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(c => c.Address)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(c => c.City)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(c => c.PhoneNumber)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.HasOne(c => c.Doctor)
                   .WithMany(d => d.Clinics)
                   .HasForeignKey(c => c.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.WorkTimes)
                   .WithOne(w => w.Clinic)
                   .HasForeignKey(w => w.ClinicId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 
