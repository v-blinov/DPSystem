using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Json.Version1
{
    public class TechincalOptions
    {
        [JsonPropertyName("engine")]
        public Engine Engine { get; set; }

        [JsonPropertyName("transmission")]
        public string Transmission { get; set; }

        [JsonPropertyName("drive")]
        public string Drive { get; set; }
    }
}
