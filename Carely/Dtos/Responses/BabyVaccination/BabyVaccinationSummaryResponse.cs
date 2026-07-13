namespace Carely.Dtos.Responses.BabyVaccination
{
    public class BabyVaccinationSummaryResponse
    {
        public int Total { get; set; }
        public int Checked { get; set; }
        public int Unchecked { get; set; }
        public int Late { get; set; }
        public int Upcoming { get; set; }
    }
}