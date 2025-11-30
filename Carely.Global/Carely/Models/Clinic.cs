using Carely.Models.Base;

namespace Carely.Models
{
    public class Clinic : Entity
    {
        public string? Name { get; set; }
        public string? Address { get; set; } 
        public string? City { get; set; }
        public string? PhoneNumber { get; set; }
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }  
        public ICollection<ClinicWorkTime> WorkTimes { get; set; } = new List<ClinicWorkTime>();
    }
}
