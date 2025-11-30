using Carely.Models.Enums.Doctor;

namespace Carely.Dtos.Requests.Doctor
{
    public class UpdateDoctorRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public string? Link { get; set; }
        public Specification Specification { get; set; }
    }

}
