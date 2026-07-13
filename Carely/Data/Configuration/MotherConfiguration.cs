using Carely.Models;
using Carely.Models.Enums.User;
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

            builder.Property(m => m.PasswordHash)
                .HasMaxLength(250)
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

            builder.HasMany(m => m.Meetings)
               .WithMany(me => me.Mothers)
               .UsingEntity(j => j.ToTable("MotherMeetings"));

            builder.HasMany(m => m.Feedbacks)
                   .WithOne(f => f.Mother)
                   .HasForeignKey(f => f.MotherId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(m => m.DeviceToken)
            .IsRequired(false) 
            .HasMaxLength(500); 


            //builder.HasData(LoadData());
        }

        //private Mother[] LoadData()
        //{
        //    return new Mother[]
        //    {
        //        //new Mother
        //        //{
        //        //    Id = 1,
        //        //    FirstName = "Sara",
        //        //    LastName = "Khaled",
        //        //    Email = "sara@example.com",
        //        //    PasswordHash = "123456",
        //        //    PhoneNumber = "01112345678",
        //        //    Role = UserRole.Mother,
        //        //    BirthDate = new DateTime(1998, 5, 10),
        //        //    Hight = 165,
        //        //    Weight = 62
        //        //},
        //        //new Mother
        //        //{
        //        //    Id = 2,
        //        //    FirstName = "Nada",
        //        //    LastName = "Mohsen",
        //        //    Email = "nada@example.com",
        //        //    PasswordHash = "654321",
        //        //    PhoneNumber = "01098765432",
        //        //    Role = UserRole.Mother,
        //        //    BirthDate = new DateTime(1995, 7, 15),
        //        //    Hight = 160,
        //        //    Weight = 58
        //        //},
        //        //new Mother
        //        //{
        //        //    Id = 3,
        //        //    FirstName = "Eman",
        //        //    LastName = "Ali",
        //        //    Email = "eman@example.com",
        //        //    PasswordHash = "987654",
        //        //    PhoneNumber = "01234567890",
        //        //    Role = UserRole.Mother,
        //        //    BirthDate = new DateTime(2000, 2, 20),
        //        //    Hight = 170,
        //        //    Weight = 70
        //        //}

        //        //server
        //        // new Mother
        //        //{
        //        //    Id = 2,
        //        //    FirstName = "Nada",
        //        //    LastName = "Mohsen",
        //        //    Email = "nada@example.com",
        //        //    PasswordHash = "654321", // keep consistent with your schema
        //        //    PhoneNumber = "01098765432",
        //        //    Role = UserRole.Mother,
        //        //    BirthDate = new DateTime(1995, 7, 15),
        //        //    Hight = 160,
        //        //    Weight = 58
        //        //},
        //        //new Mother
        //        //{
        //        //    Id = 3,
        //        //    FirstName = "Eman",
        //        //    LastName = "Ali",
        //        //    Email = "eman@example.com",
        //        //    PasswordHash = "987654",
        //        //    PhoneNumber = "01234567890",
        //        //    Role = UserRole.Mother,
        //        //    BirthDate = new DateTime(2000, 2, 20),
        //        //    Hight = 170,
        //        //    Weight = 70
        //        //},
        //        //new Mother
        //        //{
        //        //    Id = 4,
        //        //    FirstName = "Aya",
        //        //    LastName = "Mohamed",
        //        //    Email = "aya@gmail.com",
        //        //    PasswordHash = "Aya@123",
        //        //    PhoneNumber = "01155456811",
        //        //    Role = UserRole.Mother,
        //        //    BirthDate = new DateTime(2000, 10, 20, 17, 8, 33), // matches server timestamp
        //        //    Hight = 170,
        //        //    Weight = 80
        //        //},
        //        //new Mother
        //        //{
        //        //    Id = 15,
        //        //    FirstName = "Salma",
        //        //    LastName = "Shehab",
        //        //    Email = "salmasheh69@gmail.com",
        //        //    PasswordHash = "Salma@123", // placeholder, adjust if server has actual hash
        //        //    PhoneNumber = "01010529873",
        //        //    Role = UserRole.Mother,
        //        //    BirthDate = new DateTime(2004, 9, 6),
        //        //    Hight = 160,
        //        //    Weight = 63
        //        //},
        //        //new Mother
        //        //{
        //        //    Id = 16,
        //        //    FirstName = "Sama",
        //        //    LastName = "Ahmed",
        //        //    Email = "sama2323@gmail.com",
        //        //    PasswordHash = "Sama@123", // placeholder
        //        //    PhoneNumber = "01234678549",
        //        //    Role = UserRole.Mother,
        //        //    BirthDate = new DateTime(2004, 1, 15),
        //        //    Hight = 160,
        //        //    Weight = 55
        //        //}







        //    };
        //}
    }
}
