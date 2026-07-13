using Carely.Models.Base;
using Carely.Models.Enums.DetectionSession;
namespace Carely.Models
{
    public class DetectionSession :Entity
    {
        //this to generate the endpoint for the flutter the start and stop
        public  DateTime StartTime {  get; set; }
        public DateTime? EndTime { get; set; }

        public Status Status { get; set; }

        public int BabyId { get; set; }
        public Baby? Baby { get; set; }

        public ICollection<CryDetectionResult> CryDetectionResults { get; set; } = new List<CryDetectionResult>();
    }
}
