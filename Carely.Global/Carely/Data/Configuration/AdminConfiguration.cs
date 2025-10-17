using Carely.Models;
using Carely.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class AdminConfiguration : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admins");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.FirstName)
                   .HasMaxLength(50);

            builder.Property(a => a.LastName)
                   .HasMaxLength(50);

            builder.Property(a => a.Email)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(a => a.PasswordHash)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(a => a.PhoneNumber)
                   .HasMaxLength(20);

            builder.Property(a => a.Role)
                    .HasConversion<int>();

            builder.HasData(LoadData());
                
        }

        private Admin[] LoadData()
        {
            return new Admin[]
            {
                new Admin
                {
                    Id = 1,
                    FirstName = "Super",
                    LastName = "Admin",
                    Email = "super.admin@babycare.com",
                    PasswordHash = "Admin@123",
                    PhoneNumber = "01000000000",
                    Role = UserRole.Admin
                },
                new Admin
                {
                    Id = 2,
                    FirstName = "Mona",
                    LastName = "Adel",
                    Email = "mona.admin@babycare.com",
                    PasswordHash = "Mona@123",
                    PhoneNumber = "01011111111",
                    Role = UserRole.Admin
                },
                new Admin
                {
                    Id = 3,
                    FirstName = "Hassan",
                    LastName = "Tarek",
                    Email = "hassan.admin@babycare.com",
                    PasswordHash = "Hassan@123",
                    PhoneNumber = "01022222222",
                    Role = UserRole.Admin
                }
            };
        }
    }
}
