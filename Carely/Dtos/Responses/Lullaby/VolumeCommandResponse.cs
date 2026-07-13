namespace Carely.Dtos.Responses.Lullaby
{
    public class VolumeCommandResponse
    {
        public string Command { get; set; } = "Volume";  
        public int Level { get; set; }
    }
}
