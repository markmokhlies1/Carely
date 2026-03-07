using Carely.Models;
using Carely.Models.Enums.Doctor;
using Carely.Models.Enums.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.FirstName)
                   .HasMaxLength(25)
                   .IsRequired();

            builder.Property(d => d.LastName)
                   .HasMaxLength(25)
                   .IsRequired();

            builder.Property(d => d.Email)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(d => d.PasswordHash)
                   .HasMaxLength(250)
                   .IsRequired();

            builder.Property(d => d.PhoneNumber)
                   .HasMaxLength(20)
                   .IsRequired();

            builder.Property(d => d.Gender)
                   .IsRequired();

            builder.Property(d => d.Age)
                   .IsRequired();

            builder.Property(d => d.Link)
                .IsRequired();

            builder.Property(d => d.Specification)
                   .IsRequired();

            builder.Property(d => d.Role)
                   .IsRequired();

            builder.HasMany(d => d.Clinics)
                   .WithOne(c => c.Doctor)
                   .HasForeignKey(c => c.DoctorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.Meetings)
                    .WithOne(m => m.Doctor)
                    .HasForeignKey(m => m.DoctorId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(LoadData());
        }

        private Doctor[] LoadData()
            {
                return new Doctor[]
                {
                    new Doctor
                    {
                        Id = 1,
                        FirstName = "Ahmed",
                        LastName = "Samir",
                        Email = "ahmed.samir@clinic.com",
                        PasswordHash = "Doctor@123",
                        PhoneNumber = "01033333333",
                        Role = UserRole.Doctor,
                        Gender = Gender.Male,
                        Age = 40,
                        Link = "bbbb",
                        Specification = Specification.Pediatrician
                    },
                    new Doctor
                    {
                        Id = 2,
                        FirstName = "Mariam",
                        LastName = "Magdy",
                        Email = "mariam.magdy@clinic.com",
                        PasswordHash = "Mariam@123",
                        PhoneNumber = "01044444444",
                        Role = UserRole.Doctor,
                        Gender = Gender.Female,
                        Age = 35,
                        Link ="gergre",
                        Specification = Specification.Psychologist
                    }
            };
        }
    }
} 
