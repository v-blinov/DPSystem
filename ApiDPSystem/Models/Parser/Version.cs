using System.Text.Json.Serialization;

namespace ApiDPSystem.Models.Parser
{
    public class Version
    {
        [JsonPropertyName("version")]
        public int VersionValue { get; set; }
    }
}
