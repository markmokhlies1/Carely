using Carely.Dtos.Requests.ClinicWorkTime;

namespace Carely.Dtos.Requests.Clinic
{
    public class UpdateClinicRequest
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PhoneNumber { get; set; } 
        public List<UpdateClinicWorkTimeRequest>? WorkTimes { get; set; }
    }

}
