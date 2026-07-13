using Carely.Models;

namespace Carely.Dtos.Responses.Lullaby
{
    public class LullabiesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public TimeSpan? LastPosition { get; set; }
        //public string FilePath { get; set; } = string.Empty;
        public string AudioUrl { get; set; } = string.Empty;



        public string DurationFormatted => $"{Duration.Minutes:D2}:{Duration.Seconds:D2}";

        public static LullabiesResponse FromEntity(Carely.Models.Lullaby lullaby) =>
            new LullabiesResponse
            {
                Id = lullaby.Id,
                Name = lullaby.Name,
                Duration = lullaby.Duration,
                LastPosition = lullaby.LastPosition,
                AudioUrl = lullaby.FilePath
            };

       
    }
}
