using Carely.Models.Base;
using Carely.Models.Enums.Baby;

namespace Carely.Models
{
    public class Baby : Baby_Content
    {
        public DateTime DateOfBirth { get; set; }
        public int Weight { get; set; }
        public Baby_Gender Gender { get; set; }

        public Developmental Developmental { get; set; }

        public int MotherId { get; set; }
        public Mother? Mother { get; set; }

        public ICollection<BabyVaccination> BabyUsage { get; set; } = new List<BabyVaccination>();

        public ICollection<DetectionSession> DetectionSessions { get; set; } = new List<DetectionSession>();

    }
}
