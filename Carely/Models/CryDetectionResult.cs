using Carely.Models.Base;

namespace Carely.Models
{
    public class CryDetectionResult : Entity
    {
        public bool IsCrying { get; set; }
        public DateTime DetectedAt { get; set; }

        public int DetectionSessionId { get; set; }
        public DetectionSession? DetectionSession { get; set; }
    }
}
