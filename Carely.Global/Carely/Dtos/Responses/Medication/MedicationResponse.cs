using Carely.Models;

namespace Carely.Dtos.Responses.Medication
{
    public class MedicationResponse
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Spot Spot { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }

        public DateTime WillEndAt => StartDate.AddDays(Duration);

        public MedicationType MedicationType { get; set; }

        public static MedicationResponse FromEntity(Medication medication)
        {
            return new MedicationResponse
            {
                Name = medication.Name,
                Description = medication.Description,
                Spot = medication.Spot,
                StartDate = medication.StartDate,
                Duration = medication.Duration,
                MedicationType = medication.MedicationType
                // WillEndAt is auto-calculated
            };
        }
    }
}
