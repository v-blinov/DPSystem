using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Json.UniversalReadVersion
{
    public record Engine()
    {
        [JsonPropertyName("fuel")]
        public string Fuel { get; set; }

        [JsonPropertyName("power")]
        public string Power { get; set; }

        [JsonPropertyName("capacity")]
        public string Capacity { get; set; }
    }
}