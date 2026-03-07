namespace Carely.Dtos.Requests.ClinicWorkTime
{
    public class UpdateClinicWorkTimeRequest
    {
        public DayOfWeek? Day { get; set; }
        public TimeSpan? From { get; set; }
        public TimeSpan? To { get; set; }
    } 

}
