using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiDPSystem.FileFormat.Json.UniversalReadVersion
{
    public class OtherOptions
    {
        [JsonPropertyName("exterior")]
        public List<string> Exterior { get; set; }

        [JsonPropertyName("interior")]
        public List<string> Interior { get; set; }

        [JsonPropertyName("safety")]
        public List<string> Safety { get; set; }

        [JsonPropertyName("comfort")]
        public List<string> Comfort { get; set; }

        [JsonPropertyName("multimedia")]
        public List<string> Multimedia { get; set; }
    }
}