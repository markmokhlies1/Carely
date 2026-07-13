using Carely.Models.Base;

namespace Carely.Models
{
    public class BabyVaccination : Entity
    {
        public int BabyId { get; set; }
        public Baby? Baby { get; set; }

        public int VaccinationId { get; set; }
        public Vaccination? Vaccination { get; set; }

        public bool Checkbox { get; set; }

    }
}
