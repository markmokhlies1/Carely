using System.ComponentModel.DataAnnotations;

namespace Carely.Dtos.Requests.Lullaby
{
    public class CreateMotherLullabyUsageRequest
    {
        [Required] 
        public int MotherId { get; set; }

        [Required] 
        public int LullabyId { get; set; }
        public int PlayCount { get; set; } = 0;
    }
}
