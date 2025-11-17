using Carely.Models.Base;

namespace Carely.Models
{
    public class Feedback : Entity
    {
        public int Stars { get; set; } 
        public string? Comment { get; set; }
        public int MotherId { get; set; }
        public Mother? Mother { get; set; }
        public int MeetingId { get; set; }
        public Meeting? Meeting { get; set; }
    }
}
