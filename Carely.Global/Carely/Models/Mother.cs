using Carely.Models.Base;

namespace Carely.Models
{
    public class Mother : User
    {
        public DateTime BirthDate { get; set; }
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;

                if (BirthDate.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }

        public int Hight {  get; set; }
        public int Weight {  get; set; }
        public ICollection<Medication> Medications { get; set; } = new List<Medication>();
    }
}
