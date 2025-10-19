using Carely.Models.Base;
using Carely.Models.Enums;

namespace Carely.Models
{
    public class Medication : Entity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Spot Spot { get; set; }
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
        public MedicationType MedicationType { get; set; }
        public int MotherId { get; set; }
        public Mother? Mother { get; set; } 
    }
}
