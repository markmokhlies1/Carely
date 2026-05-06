using Carely.Models.Base;

namespace Carely.Models
{
    public class MotherLullabyUsage : Entity
    {
        public int MotherId { get; set; }
        public Mother Mother { get; set; } = null!;

        public int LullabyId { get; set; }
        public Lullaby Lullaby { get; set; } = null!;

        public int PlayCount { get; set; }

       
        public TimeSpan? LastPosition { get; set; }

        public bool IsPlaying { get; set; }

        public int VolumeLevel { get; set; } = 50;
    }
}

