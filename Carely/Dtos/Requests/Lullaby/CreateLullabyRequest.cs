using System.ComponentModel.DataAnnotations;

namespace Carely.Dtos.Requests.Lullaby
{
    public class CreateLullabyRequest
    {
        [Required]
        public required string Name { get; set; }


        [Required]
        public required TimeSpan Duration { get; set; }
        

        [Required]
        public required IFormFile AudioFile { get; set; }
    }
}
