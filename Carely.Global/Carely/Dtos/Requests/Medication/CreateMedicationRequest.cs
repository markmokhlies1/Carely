using Carely.Dtos.Requests.Admin;
using Carely.Models.Enums.Medication;

namespace Carely.Dtos.Requests.Medication
{
    public class CreateMedicationRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Spot Spot { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public MedicationType MedicationType { get; set; }
    }
}
