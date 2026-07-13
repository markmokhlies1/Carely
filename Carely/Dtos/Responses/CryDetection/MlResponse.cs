using System.Text.Json.Serialization;
namespace Carely.Dtos.Responses.CryDetection
{
    public class MlResponse
    {
        [JsonPropertyName("is_crying")]
        public bool IsCrying { get; set; }
    }
}
