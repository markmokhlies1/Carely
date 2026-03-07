namespace Carely.Dtos.Requests.Lullaby
{
    public class UpdateLullabyRequest
    {
        public string? Name { get; set; }
        public TimeSpan? Duration { get; set; }
        
  
        public  IFormFile? AudioFile { get; set; }
    }
}
