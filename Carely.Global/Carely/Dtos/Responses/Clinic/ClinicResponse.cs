using Carely.Dtos.Responses.ClinicWorkTime;

namespace Carely.Dtos.Responses.Clinic
{
    public class ClinicResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? PhoneNumber { get; set; }
        public List<ClinicWorkTimeResponse> WorkTimes { get; set; } = new();

        public static ClinicResponse FromEntity(Models.Clinic clinic) =>
            new ClinicResponse
            {
                Id = clinic.Id,
                Name = clinic.Name,
                Address = clinic.Address,
                City = clinic.City,
                PhoneNumber = clinic.PhoneNumber,
                WorkTimes = clinic.WorkTimes.Select(ClinicWorkTimeResponse.FromEntity).ToList()
            };
    }
}
