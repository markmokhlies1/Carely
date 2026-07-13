using Carely.Models.Enums.Baby;

namespace Carely.Dtos.Responses.Baby
{
    public class BabyResponse
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public Baby_Gender Gender { get; set; }
        public int Weight { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Developmental Developmental { get; set; }

        public static BabyResponse FromEntity(Models.Baby b)
        {
            return new BabyResponse
            {
                Id = b.Id,
                FirstName = b.FirstName,
                LastName = b.LastName,
                Gender = b.Gender,
                Weight = b.Weight,
                DateOfBirth = b.DateOfBirth,
                Developmental = b.Developmental

            };
        }
    }
}
