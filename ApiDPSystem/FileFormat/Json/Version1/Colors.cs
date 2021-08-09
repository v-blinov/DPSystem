using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Json.Version1
{
    public class Colors
    {
        [JsonPropertyName("interior")]
        public string Interior { get; set; }

        [JsonPropertyName("exterior")]
        public string Exterior { get; set; }
    }
}