using Carely.Dtos.Requests.ClinicWorkTime;

namespace Carely.Dtos.Requests.Clinic
{
    public class CreateClinicRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; } 
        public string? PhoneNumber { get; set; }
        public List<CreateClinicWorkTimeRequest> WorkTimes { get; set; } = new();
    }
}
