using Carely.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class BabyConfiguration : IEntityTypeConfiguration<Baby>
    {
        public void Configure(EntityTypeBuilder<Baby> builder)
        {
            builder.ToTable("Babies");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.FirstName)
                .HasMaxLength(25)
                .IsRequired();

            builder.Property(b =>b.LastName)
                .HasMaxLength(25)
                .IsRequired();

            builder.Property(b => b.Gender)
              .IsRequired();

            builder.Property(b => b.DateOfBirth)
                .IsRequired();

            builder.Property(b => b.Weight)
                .IsRequired();

          

            builder.Property(b => b.Developmental)
                .IsRequired();

            builder.HasOne(m => m.Mother)
                .WithMany(b => b.Babies)
                .HasForeignKey(m => m.MotherId)
                .OnDelete(DeleteBehavior.Cascade);

        }
        }
}
