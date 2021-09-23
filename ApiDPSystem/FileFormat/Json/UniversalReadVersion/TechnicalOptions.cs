using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Json.UniversalReadVersion
{
    public record TechnicalOptions()
    {
        [JsonPropertyName("engine")]
        public Engine Engine { get; set; }

        [JsonPropertyName("transmission")]
        public string Transmission { get; set; }

        [JsonPropertyName("drive")]
        public string Drive { get; set; }
    }
}