using Carely.Models.Enums.Meeting;

namespace Carely.Dtos.Requests.Meeting
{
    public class UpdateMeetingRequest
    {
        public string? Description { get; set; }
        public MeetingType? MeetingType { get; set; }
        public DateTime? Date { get; set; }
        public string? Address { get; set; } 
        public int? DoctorId { get; set; }
    }
}
