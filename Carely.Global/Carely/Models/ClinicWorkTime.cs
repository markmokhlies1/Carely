using Carely.Models.Base;

namespace Carely.Models
{
    public class ClinicWorkTime : Entity
    {
        public DayOfWeek Day { get; set; }      
        public TimeSpan From { get; set; }      
        public TimeSpan To { get; set; }        
        public int ClinicId { get; set; } 
        public Clinic? Clinic { get; set; }  
    }
}
