using Carely.Models;
using Microsoft.VisualBasic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Carely.Dtos.Responses.BabyVaccination
{
    public class BabyVaccinationResponse
    {
        public int VaccinationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Disease { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public bool Checkbox { get; set; }

        public DateTime? DueDate { get; set; }

        public static BabyVaccinationResponse FromEntity(Carely.Models.Vaccination v, bool isChecked, DateTime? dueDate = null)
        {
            return new BabyVaccinationResponse
            {
                VaccinationId = v.Id,
                Name = v.Name ?? "",
                Disease = v.Disease ?? "",
                Dosage = v.Dosage.ToString(),
                Age = v.Age.ToString(),
                Checkbox = isChecked,
                DueDate = dueDate
            };
        }
    }
}