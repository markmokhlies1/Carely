using Carely.Models.Enums.Doctor;

namespace Carely.Dtos.Responses.Doctor
{
    public class DoctorResponse
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public string? Link { get; set; }
        public Specification Specification { get; set; }

        public static DoctorResponse FromEntity(Models.Doctor d)
        {
            return new DoctorResponse
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
                PhoneNumber = d.PhoneNumber,
                Gender = d.Gender,
                Age = d.Age,
                Link = d.Link,
                Specification = d.Specification
            };
        }
    }
}
