using Carely.Models;
namespace Carely.Dtos.Responses.Mother
{
    public class MotherResponse
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public int Hight { get; set; }
        public int Weight { get; set; }

        public static MotherResponse FromEntity(Models.Mother mother)
        {
            return new MotherResponse
            {
                Id = mother.Id,
                FirstName = mother.FirstName,
                LastName = mother.LastName,
                Email = mother.Email,
                PhoneNumber = mother.PhoneNumber,
                BirthDate = mother.BirthDate,
                Hight = mother.Hight,
                Weight = mother.Weight
            };
        }
    }
}
