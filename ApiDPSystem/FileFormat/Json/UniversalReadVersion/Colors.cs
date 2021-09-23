using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Json.UniversalReadVersion
{
    public record Colors()
    {
        [JsonPropertyName("interior")]
        public string Interior { get; set; }

        [JsonPropertyName("exterior")]
        public string Exterior { get; set; }
    }
}