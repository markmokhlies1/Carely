using Carely.Models;
using Carely.Models.Enums.Vaccination;


namespace Carely.Dtos.Responses.Vaccination
{
    public class VaccinationResponse
    {
        public int Id { get; set; }
        public Age Age { get; set; }

        public Dosage Dosage { get; set; }
        public string? Name { get; set; }

        public string? Disease { get; set; }

        public static VaccinationResponse FromEntity(Models.Vaccination v)
        {
            return new VaccinationResponse
            {
                Id = v.Id,
                Age = v.Age,
                Dosage = v.Dosage,
                Name = v.Name,
                Disease = v.Disease
            };
        }
    }
}