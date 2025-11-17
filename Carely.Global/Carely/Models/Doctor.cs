using Carely.Models.Base;
using Carely.Models.Enums.Doctor;

namespace Carely.Models
{
    public class Doctor : User
    {
        public Gender Gender { get; set; }
        public int Age {  get; set; }
        public Specification Specification { get; set; }
        public ICollection<Clinic> Clinics { get; set; } = new List<Clinic>();
        public ICollection<Meeting> Meetings { get; set; } = new List<Meeting>();
    }
}
