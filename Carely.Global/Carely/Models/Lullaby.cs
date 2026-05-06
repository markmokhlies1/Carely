using Carely.Models.Base;
using System;

namespace Carely.Models
{
    public class Lullaby : Entity
    {
        public string Name { get; set; } = string.Empty;   
        public TimeSpan Duration { get; set; }             
                   
        public TimeSpan? LastPosition { get; set; }       
        public string FilePath { get; set; } = string.Empty;

        public ICollection<MotherLullabyUsage> MotherUsages { get; set; } = new List<MotherLullabyUsage>();

      
    }
}
