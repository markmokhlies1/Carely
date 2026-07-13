using Carely.Models;
using Carely.Models.Enums.Vaccination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Carely.Data.Configuration
{
    public class VaccinationConfiguration : IEntityTypeConfiguration<Vaccination>
    {

        public void Configure(EntityTypeBuilder<Vaccination> builder)
        {
            builder.ToTable("Vaccinations");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Age)
                .IsRequired();


            builder.Property(v => v.Dosage)
                .IsRequired();

            builder.Property(v => v.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(v => v.Disease)
                .IsRequired();

            builder.HasData(LoadData());

        }

        private static List<Vaccination> LoadData()
        {
            return new List<Vaccination> {

                new Vaccination {
                    Id = 1,
                    Age = Age.At_Birth,
                    Dosage = Dosage.Birth_dose,
                    Name = "Liver B infant",
                    Disease = "Hepatitis B"
                },


                new Vaccination {
                    Id = 2,
                    Age = Age.At_Birth,
                    Dosage = Dosage.Zero_dose,
                    Name = "Sabine",
                    Disease = "Polio"
                },


                new Vaccination {
                    Id = 3,
                    Age = Age.At_Birth,
                    Dosage = Dosage.Tuberculosis_dose,
                    Name = "BCG",
                    Disease = "Tuberculosis"
                },


                new Vaccination {
                    Id = 4,
                    Age = Age.Two_months,
                    Dosage =Dosage.First_dose,
                    Name = "Sabine",
                    Disease = "Polio"
                },

                new Vaccination {
                    Id = 5,
                    Age = Age.Two_months,
                    Dosage = Dosage.First_dose,
                    Name = "The taste of the pentagram",
                    Disease = "Diphtheria, pertussis, tetanus, hepatitis B and influenzae influenzae Hemorrhagic TypeB"
                },

                new Vaccination {
                    Id = 6,
                    Age = Age.Two_months,
                    Dosage = Dosage.First_dose,
                    Name = "Salk's Taste",
                    Disease = "Paralyzed polio" },


                new Vaccination {
                    Id = 7,
                    Age = Age.Four_months,
                    Dosage = Dosage.Second_dose,
                    Name = "Sabine",
                    Disease = "Polio" },

                new Vaccination { Id = 8,
                    Age = Age.Four_months,
                    Dosage = Dosage.Second_dose,
                    Name = "The taste of the pentagram",
                    Disease = "Diphtheria, whooping cough, tetanus, hepatitis B and haemorrhagic influenzae StyleB"
                },

                new Vaccination {
                    Id = 9,
                    Age = Age.Four_months,
                    Dosage = Dosage.Second_dose,
                    Name = "Taste of Soulk",
                    Disease = "Paralyzed polio"
                },


                new Vaccination {
                    Id = 10,
                    Age = Age.Six_months,
                    Dosage = Dosage.Third_dose,
                    Name = "Sabine",
                    Disease = "Polio"
                },

                new Vaccination {
                    Id = 11,
                    Age = Age.Six_months,
                    Dosage = Dosage.Third_dose,
                    Name = "The taste of the pentagram",
                    Disease = "Diphtheria, pertussis, tetanus, hepatitis B and influenzae influenzae Hemorrhagic TypeB"
                },

                new Vaccination {
                    Id = 12,
                    Age = Age.Six_months,
                    Dosage = Dosage.Third_dose,
                    Name = "Taste of Soulk",
                    Disease = "Paralyzed polio"
                },


                new Vaccination {
                    Id = 13,
                    Age = Age.Nine_months,
                    Dosage = Dosage.Fourth_dose,
                    Name = "Sabine",
                    Disease = "Polio"
                },


                new Vaccination {
                    Id = 14,
                    Age = Age.Twelve_months,
                    Dosage = Dosage.Fifth_dose,
                    Name = "Sabine",
                    Disease = "Polio"
                },


                new Vaccination {
                    Id = 15,
                    Age = Age.Twelve_months,
                    Dosage = Dosage.Stimulant_dose,
                    Name = "Viral MMR",
                    Disease = "Measles, mumps and rubella"
                },
                  new Vaccination {
                    Id = 16,
                    Age = Age.Eighteen_months,
                    Dosage = Dosage.Stimulant_dose,
                    Name = "Sabine",
                    Disease = "Polio"
                },
                  new Vaccination {
                    Id = 17,
                    Age = Age.Eighteen_months,
                    Dosage = Dosage.Stimulant_dose,
                    Name = "Viral MMR",
                    Disease = "Measles, mumps and rubella"
                },
                   new Vaccination {
                    Id = 18,
                    Age = Age.Eighteen_months,
                    Dosage = Dosage.Stimulant_dose,
                    Name = "Bacterial triad",
                    Disease = "Diphtheria, tetanus, and whooping cough"
                }


            };


        }
    }
}
