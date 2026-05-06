using Carely.Models.Base;
using Carely.Models.Enums.Vaccination;

namespace Carely.Models
{
    public class Vaccination : Entity
    {
        public Age Age { get; set; }

       public Dosage Dosage { get; set; }
        public string? Name { get; set; }

        public string? Disease { get; set; }

        public ICollection<BabyVaccination> VaccinationUsage { get; set; } = new List<BabyVaccination>();



    }
}
