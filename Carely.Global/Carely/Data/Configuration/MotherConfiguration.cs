using Carely.Models;
using Carely.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class MotherConfiguration : IEntityTypeConfiguration<Mother>
    {
        public void Configure(EntityTypeBuilder<Mother> builder)
        {
            builder.ToTable("Mothers");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.FirstName)
                .HasMaxLength(25)
                .IsRequired();

            builder.Property(m => m.LastName)
                .HasMaxLength(25)
                .IsRequired();

            builder.Property(m => m.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.Password)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(m => m.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(m => m.BirthDate)
                .IsRequired();

            builder.Property(m => m.BirthDate)
                   .IsRequired();

            builder.Property(m => m.Role)
                   .IsRequired();


            builder.Property(m => m.Hight)
                   .IsRequired();

            builder.Property(m => m.Weight)
                   .IsRequired();

            builder.Ignore(m => m.Age);
            builder.HasData(LoadData());
        }

        private Mother[] LoadData()
        {
            return new Mother[]
            {
                new Mother
                {
                    Id = 1,
                    FirstName = "Sara",
                    LastName = "Khaled",
                    Email = "sara@example.com",
                    Password = "123456",
                    PhoneNumber = "01112345678",
                    Role = UserRole.Mother,
                    BirthDate = new DateTime(1998, 5, 10),
                    Hight = 165,
                    Weight = 62
                },
                new Mother
                {
                    Id = 2,
                    FirstName = "Nada",
                    LastName = "Mohsen",
                    Email = "nada@example.com",
                    Password = "654321",
                    PhoneNumber = "01098765432",
                    Role = UserRole.Mother,
                    BirthDate = new DateTime(1995, 7, 15),
                    Hight = 160,
                    Weight = 58
                },
                new Mother
                {
                    Id = 3,
                    FirstName = "Eman",
                    LastName = "Ali",
                    Email = "eman@example.com",
                    Password = "987654",
                    PhoneNumber = "01234567890",
                    Role = UserRole.Mother,
                    BirthDate = new DateTime(2000, 2, 20),
                    Hight = 170,
                    Weight = 70
                }
            };
        }
    }
}
