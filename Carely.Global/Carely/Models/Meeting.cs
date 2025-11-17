using Carely.Models.Base;
using Carely.Models.Enums.Meeting;

namespace Carely.Models
{
    public class Meeting : Entity
    {
        public string? Description { get; set; }
        public MeetingType MeetingType { get; set; }
        public DateTime Date { get; set; }
        public string? Address { get; set; }
        public ICollection<Mother> Mothers { get; set; } = new List<Mother>();
        public int DoctorId { get; set; }
        public Doctor? Doctor { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    }
}
