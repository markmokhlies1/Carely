using Carely.Models.Base;

namespace Carely.Models
{
    public class Mother : User
    {
        public DateTime BirthDate { get; set; }
        public int Hight {  get; set; }
        public int Weight {  get; set; }
        public ICollection<Medication> Medications { get; set; } = new List<Medication>();
        public ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
        public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>(); 
    }
}
