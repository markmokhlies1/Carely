using Carely.Models.Enums;
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
        public DateTime WillEndAt {  get; set; }
        public MedicationType MedicationType { get; set; }

        public static MedicationResponse? FromEntity(Models.Medication medication)
        {
            return new MedicationResponse
            {
                Name = medication.Name,
                Description = medication.Description,
                Spot = medication.Spot,
                StartDate = medication.StartDate,
                Duration = medication.Duration,
                WillEndAt = medication.StartDate.AddDays(medication.Duration)
            };
        }
    }
}
