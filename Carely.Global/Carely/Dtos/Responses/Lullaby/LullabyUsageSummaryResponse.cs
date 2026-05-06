namespace Carely.Dtos.Responses.Lullaby
{
    public class LullabyUsageSummaryResponse
    {
        public int LullabyId { get; set; }
        public string LullabyName { get; set; } = string.Empty;
        public int MotherCount { get; set; }
        public List<MotherUsageResponse> Mothers { get; set; } = new();
    }
}
