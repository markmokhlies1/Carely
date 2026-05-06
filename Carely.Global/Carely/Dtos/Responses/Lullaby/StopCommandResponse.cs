namespace Carely.Dtos.Responses.Lullaby
{
    public class StopCommandResponse
    {
        public string Command { get; set; } = "STOP";
        public TimeSpan StopPosition { get; set; }
    }
}
