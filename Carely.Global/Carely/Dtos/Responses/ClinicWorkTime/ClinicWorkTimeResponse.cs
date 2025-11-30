namespace Carely.Dtos.Responses.ClinicWorkTime
{
    public class ClinicWorkTimeResponse
    {
        public DayOfWeek Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }

        public static ClinicWorkTimeResponse FromEntity(Models.ClinicWorkTime wt) =>
            new ClinicWorkTimeResponse
            {
                Day = wt.Day,
                From = wt.From,
                To = wt.To
            };
    }
}
