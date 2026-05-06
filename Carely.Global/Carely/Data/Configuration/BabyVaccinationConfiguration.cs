using Carely.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class BabyVaccinationConfiguration : IEntityTypeConfiguration<BabyVaccination>
    {
        public void Configure(EntityTypeBuilder<BabyVaccination> builder)
        {
           
            builder.ToTable("BabyVaccination");

          
            builder.HasKey(bv => bv.Id);

    
            builder.HasOne(bv => bv.Baby)
                   .WithMany(b => b.BabyUsage)
                   .HasForeignKey(bv => bv.BabyId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bv => bv.Vaccination)
                   .WithMany(v => v.VaccinationUsage)
                   .HasForeignKey(bv => bv.VaccinationId)
                   .OnDelete(DeleteBehavior.Cascade);

       
            builder.Property(bv => bv.Checkbox)
                   .IsRequired();
        }
    }
}
