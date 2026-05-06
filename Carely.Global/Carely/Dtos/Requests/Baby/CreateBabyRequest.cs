using Carely.Models.Enums.Baby;

namespace Carely.Dtos.Requests.Baby
{
    public class CreateBabyRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public Baby_Gender Gender { get; set; }
        public int Weight { get; set; }

        public DateTime DateOfBirth { get; set; }

        public Developmental Developmental { get; set; }

    }
}
