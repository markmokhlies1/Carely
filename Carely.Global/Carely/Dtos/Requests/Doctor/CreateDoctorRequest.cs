using Carely.Models.Enums.Doctor;
using Carely.Models.Enums.User;

namespace Carely.Dtos.Requests.Doctor
{
    public class CreateDoctorRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public string? Link { get; set; }
        public Specification Specification { get; set; }
    }
}
